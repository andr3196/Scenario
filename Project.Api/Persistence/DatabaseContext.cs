using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Domain;
using Scenario.Domain.SharedTypes;

namespace Project.Api.Persistence
{
    public class DatabaseContext : DbContext
    {
        private readonly IScenarioEventPropagator scenarioEventPropagator;

        public DatabaseContext(IScenarioEventPropagator scenarioEventPropagator)
        {
            this.scenarioEventPropagator = scenarioEventPropagator;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=/tmp/example-project.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("project");
            modelBuilder.Entity<Customer>()
                .ToTable(nameof(Customer))
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer);

            modelBuilder.Entity<Email>()
                .ToTable(nameof(Email));

            modelBuilder.Entity<Item>()
                .ToTable(nameof(Item));

            modelBuilder.Entity<Order>()
                .ToTable(nameof(Order))
                .HasMany(o => o.Products)
                .WithOne();

            modelBuilder.Entity<Product>()
                .ToTable(nameof(Product))
                .HasOne(p => p.Item)
                .WithMany()
                .HasForeignKey(p => p.ItemId);

            modelBuilder.Entity<Receipt>()
                .ToTable(nameof(Receipt));

        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            ChangeTracker.Entries<Entity>()
                .Where(e => e.State == EntityState.Added)
                .ToList()
                .ForEach(e => e.Entity.Created());

            ChangeTracker.Entries<Entity>()
                .Where(e => e.State == EntityState.Modified)
                .ToList()
                .ForEach(e => e.Entity.Updated());

            var entitiesWithEvents = ChangeTracker
                .Entries<Entity>()
                .Where(e =>
                    e.State != EntityState.Detached
                    && e.Entity.Events.Any())
                .Select(e => e.Entity);

            var result = base.SaveChanges();

            HandleEventsAsync(entitiesWithEvents).GetAwaiter().GetResult();
            try
            {
                
            }
            catch
            {
                base.Database.RollbackTransaction();
            }

            return result;
        }

        private async Task HandleEventsAsync(System.Collections.Generic.IEnumerable<Entity> entitiesWithEvents)
        {
            var propagateMethod = typeof(IScenarioEventPropagator).GetMethod(nameof(IScenarioEventPropagator.PropagateAsync));
            
            var entityEvents = entitiesWithEvents
                    .SelectMany(e => e.Events)
                    .ToList();
            foreach (var @event in entityEvents)
            {
                var eventType = @event.GetType();
                var entityType = eventType.BaseType.GetGenericArguments().First();
                var genericMethod = propagateMethod.MakeGenericMethod(eventType, entityType);
                await (Task)genericMethod.Invoke(scenarioEventPropagator, new[] { @event });
            }

            entitiesWithEvents.ToList().ForEach(e => e.ResetEvents());
        }
    }
}
