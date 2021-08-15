using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Scenario.Application;
using Scenario.Domain.Clauses;
using Scenario.Domain.Modeling.Models;
using Scenario.Domain.ScenarioDefinitions;

namespace Scenario.Services
{
    public class ScenarioParsingService : IScenarioParsingService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IScenarioModelService scenarioModelService;
        private readonly IScenarioConsequenceExpressionBuilder scenarioConsequenceExpressionBuilder;

        public ScenarioParsingService(
            IServiceProvider serviceProvider,
            IScenarioModelService scenarioModelService,
            IScenarioConsequenceExpressionBuilder scenarioConsequenceExpressionBuilder)
        {
            this.serviceProvider = serviceProvider;
            this.scenarioModelService = scenarioModelService;
            this.scenarioConsequenceExpressionBuilder = scenarioConsequenceExpressionBuilder;
        }

        public bool TryParse(ScenarioDefinitionDto scenario, out ScenarioDefinition scenarioDefinition)
        {
            try
            {
                scenarioDefinition = Parse(scenario);
                return true;
            } catch
            {
                scenarioDefinition = null;
                return false;
            }
        }

        public ScenarioDefinition Parse(ScenarioDefinitionDto definitionDto)
        {
            var model = scenarioModelService.GetModel();

            var scenarioDefinition = new ScenarioDefinition();
            scenarioDefinition.EventType = GetEventType(definitionDto.Entity, definitionDto.Event, model);
            var entityType = GetEntityType(definitionDto.Entity, model);
            scenarioDefinition.Condition = definitionDto.Condition.GetPredicateExpression(entityType).Compile();
            scenarioDefinition.Handler = scenarioConsequenceExpressionBuilder.BuildExpression(definitionDto.Consequence, model).Compile();

            return scenarioDefinition;
        }

        protected Type GetEventType(string entityKey, string eventKey, ScenarioSetup model)
        {
            var entityType = GetEntityType(entityKey, model);
            model.EventsDictionary.TryGetValue(entityType.Name, out var events);
            if (events == null)
            {
                throw new ArgumentException($"No events for entity: {entityType.Name}");
            }
            var @event = events.FirstOrDefault(e => e.Value == eventKey)
                ?? throw new ArgumentException($"No event matches key {eventKey}");
            return Type.GetType(@event.Value) ?? throw new ArgumentException($"Cannot find event for {@event.Label}");
        }

        protected Type GetEntityType(string entityKey, ScenarioSetup model)
        {
            var entityModel = model.Entities.SingleOrDefault(e => e.Value == entityKey)
                            ?? throw new ArgumentException($"No entity matches key: {entityKey}");

            var entityType = Type.GetType(entityModel.Value)
                ?? throw new ArgumentException($"Cannot find type for {entityModel.Label}");
            return entityType;
        }

    }
}
