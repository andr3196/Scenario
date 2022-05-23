using System;
using System.Linq;
using System.Linq.Expressions;
using Scenario.Domain.Modeling.Models;
using System.Threading.Tasks;
using System.Threading;
using Scenario.Contracts;
using Scenario.Domain.Shared.TypeHandling;
using Scenario.Domain.Modeling.Services;
using Scenario.Domain.Models;
using Scenario.Domain.Services.ExpressionBuilding;

namespace Scenario.Application.Services.ScenarioParsing
{
    public class ScenarioParsingService : IScenarioParsingService
    {
        private readonly IScenarioDomainService scenarioModelService;
        private readonly IConsequenceExpressionBuilder consequenceBuilder;
        private readonly IConditionExpressionBuilder conditionBuilder;
        private readonly IDomainTypeResolver domainTypeResolver;

        public ScenarioParsingService(
            IScenarioDomainService scenarioModelService,
            IConsequenceExpressionBuilder consequenceBuilder,
            IConditionExpressionBuilder conditionBuilder,
            IDomainTypeResolver domainTypeResolver)
        {
            this.scenarioModelService = scenarioModelService;
            this.consequenceBuilder = consequenceBuilder;
            this.conditionBuilder = conditionBuilder;
            this.domainTypeResolver = domainTypeResolver;
        }

        public bool TryParse(ScenarioDefinitionDto scenario, out ScenarioFlow? scenarioDefinition)
        {
            return TryParse(scenario, out scenarioDefinition, out _);
        }

        public bool TryParse(ScenarioDefinitionDto scenario, out ScenarioFlow? scenarioDefinition, out Exception? exception)
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

        public ScenarioFlow Parse(ScenarioDefinitionDto definitionDto)
        {
            var model = scenarioModelService.GetModel();
            var entityType = GetEntityType(definitionDto.Entity, model);
            var definitionType = typeof(ScenarioFlow<>).MakeGenericType(entityType);

            var scenarioDefinition = (ScenarioFlow)Activator.CreateInstance(definitionType)!;
            scenarioDefinition.EventType = GetEventType(definitionDto.Entity, definitionDto.Event, model);
            scenarioDefinition.Key = domainTypeResolver.GenerateKey(scenarioDefinition.EventType);

            var condition = GetCondition(entityType)(new object[] { definitionDto.Condition, entityType });
            var consequence = GetConsequence(entityType)(new object[] { definitionDto.Consequence, model });
            scenarioDefinition.Install(condition, consequence);
            return scenarioDefinition;
        }

        private Func<object[], object> GetCondition(Type entityType)
        {
            var conditionMethod = typeof(IConditionExpressionBuilder).GetMethod(nameof(IConditionExpressionBuilder.BuildExpression));
            var genericConditionMethod = conditionMethod!.MakeGenericMethod(entityType);
            var expressionType = typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(entityType, typeof(bool)));
            var compileMethod = expressionType.GetMethod(nameof(LambdaExpression.Compile), Array.Empty<Type>())!;
            return arguments => compileMethod.Invoke(genericConditionMethod.Invoke(conditionBuilder, arguments), new object[] { });
        }

        private Func<object[], object> GetConsequence(Type entityType)
        {
            var conditionMethod = typeof(IConsequenceExpressionBuilder).GetMethod(nameof(IConsequenceExpressionBuilder.BuildExpression));
            var genericConditionMethod = conditionMethod!.MakeGenericMethod(new Type[] { entityType });
            var expressionType = typeof(Expression<>).MakeGenericType(new Type[] { typeof(Func<,,>).MakeGenericType(entityType, typeof(CancellationToken), typeof(Task)) });
            var compileMethod = expressionType.GetMethod(nameof(LambdaExpression.Compile), new Type[] { })!;
            return arguments => {
                var expression = genericConditionMethod.Invoke(consequenceBuilder, arguments);
                var func = compileMethod.Invoke(expression, Array.Empty<object>());
                return func;
            };
        }

        protected Type GetEventType(string entityKey, string eventKey, ScenarioDomainModel model)
        {
            model.EventsDictionary.TryGetValue(entityKey, out var events);
            if (events == null)
            {
                var entityType = GetEntityType(entityKey, model);
                throw new ArgumentException($"No events for entity: {entityType.Name}");
            }
            var @event = events.FirstOrDefault(e => e.Value == eventKey)
                ?? throw new ArgumentException($"No event matches key {eventKey}");
            return domainTypeResolver.ResolveType(@event.Value) ?? throw new ArgumentException($"Cannot find event for {@event.Label}");
        }

        protected Type GetEntityType(string entityKey, ScenarioDomainModel model)
        {
            var entityModel = model.Entities.SingleOrDefault(e => e.Value == entityKey)
                ?? throw new ArgumentException($"No entity matches key: {entityKey}");

            var entityType = domainTypeResolver.ResolveType(entityModel.Value)
                ?? throw new ArgumentException($"Cannot find type for {entityModel.Label}");
            return entityType!;
        }

    }
}
