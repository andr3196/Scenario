using System.Threading;
using System.Threading.Tasks;
using Project.Domain.EventHandlers;
using Scenario.Domain.Shared.Events;
using Scenario.Events;

namespace Project.Api.Services
{
    public class ScenarioEventHandler<TEntity> : IEventHandler<IDomainEvent<TEntity>> where TEntity : class
    {
        private readonly IScenarioEventService eventService;

        public ScenarioEventHandler(IScenarioEventService eventService)
        {
            this.eventService = eventService;
        }

        public Task HandleAsync(IDomainEvent<TEntity> @event, CancellationToken cancellationToken)
        {
            return eventService.HandleEvent<IDomainEvent<TEntity>, TEntity>(@event, cancellationToken);
        }
    }
}