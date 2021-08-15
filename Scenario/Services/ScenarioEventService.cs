using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Scenario.Application;
using Scenario.Domain;
using Scenario.Domain.ScenarioDefinitions;
using Scenario.Domain.SharedTypes;
using Scenario.Infrastructure;

namespace Scenario.Services
{
    public class ScenarioEventService : IHostedService, IScenarioEventService
    {
        private readonly Dictionary<string, List<ScenarioDefinition>> definitionsState = new();
        private readonly List<Task> ongoingTasks = new List<Task>();
        private readonly IDatabaseContext databaseContext;
        private readonly IScenarioParsingService scenarioParsingService;

        public ScenarioEventService(
            IDatabaseContext databaseContext,
            IScenarioParsingService scenarioParsingService)
        {
            this.databaseContext = databaseContext;
            this.scenarioParsingService = scenarioParsingService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var definitions = (await databaseContext.Set<Domain.Scenarios.Scenario>()
                .Where(s => s.Active)
                .ToListAsync(cancellationToken))
                .Select(s =>
                {
                    var scenarioDefinitionDto = JsonSerializer.Deserialize<ScenarioDefinitionDto>(s.Description);
                    scenarioParsingService.TryParse(scenarioDefinitionDto, out var scenarioDefinition);
                    return scenarioDefinition;
                })
                .ToList();
            definitions.ForEach(d => Register(d));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // await ongoing tasks
            await Task.WhenAll(ongoingTasks);
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
            where TEntity : class, IScenarioEntity
        {
            var eventType = @event.GetType();
            if (definitionsState.ContainsKey(eventType.FullName))
            {
                var potentialScenarios = definitionsState[eventType.FullName];
                var tasks = potentialScenarios
                    .Select(s => new
                    {
                        Scenario = s,
                        Data = @event.Entity
                    })
                    .Where(x => x.Scenario.Condition(x.Data))
                    .Select(async x => await x.Scenario.Handler(x.Data, cancellationToken))
                    .Select(task => { ongoingTasks.Add(task); return task; })
                    .ToList();
                var handleAllEventsTask =  Task.WhenAll(tasks);
                await handleAllEventsTask;
                tasks.ForEach(task => ongoingTasks.Remove(task));
            }
        }
    }
}
