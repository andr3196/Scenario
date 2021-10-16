using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.Models.Scenarios;
using Scenario.Domain.Services.Persistence;
using Scenario.Domain.Shared.Events;
using Scenario.Domain.Shared.TypeHandling;

namespace Scenario.Domain.Services.EventHandling
{
    public class ScenarioEventService : IScenarioEventService
    {
        private readonly Dictionary<string, List<ScenarioDefinition>> definitionsState = new();
        private readonly List<Task> ongoingTasks = new List<Task>();
        private readonly IDomainTypeResolver domainTypeResolver;
        private readonly IScenarioRepository scenarioRepository;

        public ScenarioEventService(
            IDomainTypeResolver domainTypeResolver,
            IScenarioRepository scenarioRepository)
        {
            this.domainTypeResolver = domainTypeResolver;
            this.scenarioRepository = scenarioRepository;
        }

        public void Register(ScenarioDefinition scenarioDefinition)
        {
            if (definitionsState.ContainsKey(scenarioDefinition.Key))
            {
                definitionsState[scenarioDefinition.Key].Add(scenarioDefinition);
            }
            else
            {
                definitionsState.Add(scenarioDefinition.Key, new List<ScenarioDefinition> { scenarioDefinition });
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
                    .Select(task => { ongoingTasks.Add(task); return task; })
                    .ToList();
                var handleAllEventsTask =  Task.WhenAll(tasks);
                await handleAllEventsTask;
                tasks.ForEach(task => ongoingTasks.Remove(task));
            }
        }

        public async Task LoadAllAsync(CancellationToken cancellationToken)
        {
            var definitions = await scenarioRepository.GetAllActiveScenarioDefinitionsAsync(cancellationToken);
            definitions.ToList().ForEach(d => Register(d));
        }

        public Task WaitForTasksToCompleteAsync(CancellationToken cancellationToken)
        {
            return Task.WhenAll(ongoingTasks);
        }
    }
}
