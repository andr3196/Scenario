using System;
using System.Threading.Tasks;
using Scenario.Domain.SharedTypes;

namespace Scenario.Services
{
    public class ScenarioEventPropagator : IScenarioEventPropagator
    {
        private readonly IScenarioEventService scenarioEventService;

        public ScenarioEventPropagator(IScenarioEventService scenarioEventService)
        {
            this.scenarioEventService = scenarioEventService;
        }

        public async Task PropagateAsync<TEvent, TEntity>(TEvent @event)
            where TEvent : IDomainEvent<TEntity>
            where TEntity : class, IScenarioEntity
        {
            await scenarioEventService.HandleEvent<TEvent, TEntity>(@event, default);
        }
    }
}
