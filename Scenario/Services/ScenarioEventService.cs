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
using Scenario.Serialization;

namespace Scenario.Services
{
    public class ScenarioEventService : IHostedService, IScenarioEventService
    {
        private readonly Dictionary<string, List<ScenarioDefinition>> definitionsState = new();
        private readonly List<Task> ongoingTasks = new List<Task>();
        private readonly IDatabaseContext databaseContext;
        private readonly IScenarioParsingService scenarioParsingService;
        private readonly ISerializationService serializationService;
        private readonly IDomainTypeResolver domainTypeResolver;

        public ScenarioEventService(
            IDatabaseContext databaseContext,
            IScenarioParsingService scenarioParsingService,
            ISerializationService serializationService,
            IDomainTypeResolver domainTypeResolver)
        {
            this.databaseContext = databaseContext;
            this.scenarioParsingService = scenarioParsingService;
            this.serializationService = serializationService;
            this.domainTypeResolver = domainTypeResolver;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Started event service");
            var definitions = (await databaseContext.Set<Domain.Scenarios.Scenario>()
                .Where(s => s.Active)
                .ToListAsync(cancellationToken))
                .Select(s =>
                {
                    var scenarioDefinitionDto = serializationService.Deserialize<ScenarioDefinitionDto>(s.Description);
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
            var key = domainTypeResolver.GetKey(eventType);
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
    }
}
