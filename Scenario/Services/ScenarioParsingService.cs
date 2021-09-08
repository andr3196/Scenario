using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Scenario.Application;
using Scenario.Domain.Clauses;
using Scenario.Domain.Modeling.Models;
using Scenario.Domain.ScenarioDefinitions;
using Scenario.Services.ExpressionBuilding;
using Scenario.Domain.SharedTypes;
using Project.Domain;
using System.Threading.Tasks;
using System.Threading;

namespace Scenario.Services
{
    public class ScenarioParsingService : IScenarioParsingService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IScenarioModelService scenarioModelService;
        private readonly IScenarioConsequenceExpressionBuilder scenarioConsequenceExpressionBuilder;
        private readonly IScenarioConditionExpressionBuilder scenarioConditionExpressionBuilder;
        private readonly IDomainTypeResolver domainTypeResolver;

        public ScenarioParsingService(
            IServiceProvider serviceProvider,
            IScenarioModelService scenarioModelService,
            IScenarioConsequenceExpressionBuilder scenarioConsequenceExpressionBuilder,
            IScenarioConditionExpressionBuilder scenarioConditionExpressionBuilder,
            IDomainTypeResolver domainTypeResolver)
        {
            this.serviceProvider = serviceProvider;
            this.scenarioModelService = scenarioModelService;
            this.scenarioConsequenceExpressionBuilder = scenarioConsequenceExpressionBuilder;
            this.scenarioConditionExpressionBuilder = scenarioConditionExpressionBuilder;
            this.domainTypeResolver = domainTypeResolver;
        }

        public bool TryParse(ScenarioDefinitionDto scenario, out ScenarioDefinition? scenarioDefinition)
        {
            return TryParse(scenario, out scenarioDefinition, out _);
        }

        public bool TryParse(ScenarioDefinitionDto scenario, out ScenarioDefinition? scenarioDefinition, out Exception? exception)
        {
            try
            {
                scenarioDefinition = Parse(scenario);
                exception = null;
                return true;
            }
            catch (Exception e)
            {
                scenarioDefinition = null;
                exception = e;
                return false;
            }
        }

        public ScenarioDefinition Parse(ScenarioDefinitionDto definitionDto)
        {
            var model = scenarioModelService.GetModel();
            var entityType = GetEntityType(definitionDto.Entity, model);
            var definitionType = typeof(ScenarioDefinition<>).MakeGenericType(new Type[] { entityType });

            var scenarioDefinition = (ScenarioDefinition)Activator.CreateInstance(definitionType)!;
            scenarioDefinition.EventType = GetEventType(definitionDto.Entity, definitionDto.Event, model);
            scenarioDefinition.Key = domainTypeResolver.GetKey(scenarioDefinition.EventType);

            var condition = GetCondition(entityType)(new object[] { definitionDto.Condition, entityType });
            var consequence = GetConsequence(entityType)(new object[] { definitionDto.Consequence, model });
            scenarioDefinition.Install(condition, consequence);
            return scenarioDefinition;
        }

        private Func<object[], object> GetCondition(Type entityType)
        {
            var conditionMethod = typeof(IScenarioConditionExpressionBuilder).GetMethod(nameof(IScenarioConditionExpressionBuilder.BuildExpression));
            var genericConditionMethod = conditionMethod!.MakeGenericMethod(new Type[] { entityType });
            var expressionType = typeof(Expression<>).MakeGenericType(new Type[] { typeof(Func<,>).MakeGenericType(entityType, typeof(bool)) });
            var compileMethod = expressionType.GetMethod("Compile", new Type[] { })!;
            return arguments => compileMethod.Invoke(genericConditionMethod.Invoke(scenarioConditionExpressionBuilder, arguments), new object[] { });
        }

        private Func<object[], object> GetConsequence(Type entityType)
        {
            var conditionMethod = typeof(IScenarioConsequenceExpressionBuilder).GetMethod(nameof(IScenarioConsequenceExpressionBuilder.BuildExpression));
            var genericConditionMethod = conditionMethod!.MakeGenericMethod(new Type[] { entityType });
            var expressionType = typeof(Expression<>).MakeGenericType(new Type[] { typeof(Func<,,>).MakeGenericType(entityType, typeof(CancellationToken), typeof(Task)) });
            var compileMethod = expressionType.GetMethod("Compile", new Type[] { })!;
            return arguments => {
                var expression = genericConditionMethod.Invoke(scenarioConsequenceExpressionBuilder, arguments);
                var func = compileMethod.Invoke(expression, new object[] { });
                return func;
            };
        }

        protected Type GetEventType(string entityKey, string eventKey, ScenarioSetup model)
        {
            model.EventsDictionary.TryGetValue(entityKey, out var events);
            if (events == null)
            {
                var entityType = GetEntityType(entityKey, model);
                throw new ArgumentException($"No events for entity: {entityType.Name}");
            }
            var @event = events.FirstOrDefault(e => e.Value == eventKey)
                ?? throw new ArgumentException($"No event matches key {eventKey}");
            return domainTypeResolver.ResolveTypeFromKey(@event.Value) ?? throw new ArgumentException($"Cannot find event for {@event.Label}");
        }

        protected Type GetEntityType(string entityKey, ScenarioSetup model)
        {
            var entityModel = model.Entities.SingleOrDefault(e => e.Value == entityKey)
                            ?? throw new ArgumentException($"No entity matches key: {entityKey}");

            var entityType = domainTypeResolver.ResolveTypeFromKey(entityModel.Value)
                ?? throw new ArgumentException($"Cannot find type for {entityModel.Label}");
            return entityType!;
        }

    }
}
