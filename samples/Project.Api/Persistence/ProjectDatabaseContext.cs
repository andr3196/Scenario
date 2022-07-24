using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project.Domain;
using Project.Domain.EventHandlers;
using Scenario.Domain.Shared.Events;

namespace Project.Api.Persistence
{
    public class ProjectDatabaseContext : DbContext
    {
        private readonly IServiceProvider serviceProvider;

        public ProjectDatabaseContext(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=example-project3.db");

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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
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
                    e.State != EntityState.Detached)
                .Select(e => e.Entity);

            var transaction = await base.Database.BeginTransactionAsync(cancellationToken);
            var result = await base.SaveChangesAsync(cancellationToken);
            try
            {
                await HandleEventsAsync(entitiesWithEvents, cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
            }

            return result;
        }

        private async Task HandleEventsAsync(System.Collections.Generic.IEnumerable<Entity> entitiesWithEvents, CancellationToken cancellationToken)
        {
            var entities = entitiesWithEvents.ToList();
           var entityEvents = entities
                    .SelectMany(e => e.Events)
                    .ToList();
            foreach (var @event in entityEvents)
            {
                var handlers = serviceProvider.GetServices(typeof(IEventHandler<>).MakeGenericType(@event.GetType())).Cast<IEventHandler<IDomainEvent>>();
                foreach (var handler in handlers)
                {
                    await handler.HandleAsync(@event, cancellationToken);
                }
                
            }
            entities.ToList().ForEach(e => e.ResetEvents());
        }
    }
}
