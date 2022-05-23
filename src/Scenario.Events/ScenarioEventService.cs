using Scenario.Core.Persistence;
using Scenario.Domain.Models;
using Scenario.Domain.Shared.Events;
using Scenario.Domain.Shared.TypeHandling;

namespace Scenario.Events
{
    public class ScenarioEventService : IScenarioEventService
    {
        private readonly Dictionary<string, List<ScenarioFlow>> definitionsState = new();
        private readonly IDomainTypeResolver domainTypeResolver;
        private readonly IScenarioRepository scenarioRepository;
        private readonly IEventSynchronisationService eventSynchronisationService;

        public ScenarioEventService(
            IDomainTypeResolver domainTypeResolver,
            IScenarioRepository scenarioRepository,
            IEventSynchronisationService eventSynchronisationService)
        {
            this.domainTypeResolver = domainTypeResolver;
            this.scenarioRepository = scenarioRepository;
            this.eventSynchronisationService = eventSynchronisationService;
        }

        public void Register(ScenarioFlow scenarioFlow)
        {
            if (definitionsState.ContainsKey(scenarioFlow.Key))
            {
                definitionsState[scenarioFlow.Key].Add(scenarioFlow);
            }
            else
            {
                definitionsState.Add(scenarioFlow.Key, new List<ScenarioFlow> { scenarioFlow });
            }
        }

        public async Task HandleEvent<TEvent, TEntity>(TEvent @event, CancellationToken cancellationToken)
            where TEvent : IDomainEvent<TEntity>
            where TEntity : class
        {
            var eventType = @event.GetType();
            var key = domainTypeResolver.GenerateKey(eventType);
            if (definitionsState.ContainsKey(key))
            {
                var potentialScenarios = definitionsState[key];
                var tasks = potentialScenarios
                    .Select(s => new
                    {
                        Scenario = s,
                        Data = @event.Entity
                    })
                    .Where(x => x.Scenario.InvokeCondition(x.Data))
                    .Select(async x => await x.Scenario.InvokeConsequence(x.Data, cancellationToken))
                    .Select(task => { eventSynchronisationService.AddTask(task); return task; })
                    .ToList();
                await Task.WhenAll(tasks);
            }
        }

        public async Task LoadAllAsync(CancellationToken cancellationToken)
        {
            var definitions = await scenarioRepository.GetAllActiveScenarioDefinitionsAsync(cancellationToken);
            definitions.ToList().ForEach(Register);
        }
    }
}
