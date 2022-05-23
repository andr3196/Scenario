using System.Linq.Expressions;
using Scenario.Domain.Modeling.Models;
using Scenario.Domain.Modeling.Services;
using Scenario.Domain.Models;
using Scenario.Domain.Services.ExpressionBuilding;
using Scenario.Domain.Shared.TypeHandling;

namespace Scenario.Domain.Persistence;

public class DatabaseScenarioMapper : IDatabaseScenarioMapper
{
    private readonly IScenarioDomainService scenarioDomainService;
    private readonly IDomainTypeResolver domainTypeResolver;
    private readonly IConsequenceExpressionBuilder consequenceBuilder;
    private readonly IConditionExpressionBuilder conditionBuilder;

    public DatabaseScenarioMapper(IScenarioDomainService scenarioDomainService, IDomainTypeResolver domainTypeResolver, IConsequenceExpressionBuilder consequenceBuilder,
        IConditionExpressionBuilder conditionBuilder)
    {
        this.scenarioDomainService = scenarioDomainService;
        this.domainTypeResolver = domainTypeResolver;
        this.consequenceBuilder = consequenceBuilder;
        this.conditionBuilder = conditionBuilder;
    }
    public ScenarioFlow Map(DatabaseScenario dbScenario)
    {
        var model = scenarioDomainService.GetModel();
        var entityType = GetEntityType(dbScenario.EntityKey, model);
        var definitionType = typeof(ScenarioFlow<>).MakeGenericType(entityType);

        var scenarioDefinition = (ScenarioFlow)Activator.CreateInstance(definitionType)!;
        scenarioDefinition.EventType = GetEventType(dbScenario.EntityKey, dbScenario.EventKey, model);
        scenarioDefinition.Key = domainTypeResolver.GenerateKey(scenarioDefinition.EventType);

        var condition = GetCondition(entityType)(new object[] { dbScenario.ConditionJson, entityType });
        var consequence = GetConsequence(entityType)(new object[] { dbScenario.ConsequenceJson, model });
        scenarioDefinition.Install(condition, consequence);
        return scenarioDefinition;
    }
    
    private Func<object[], object> GetCondition(Type entityType)
    {
        var conditionMethod = typeof(IConditionExpressionBuilder).GetMethod(nameof(IConditionExpressionBuilder.BuildExpression));
        var genericConditionMethod = conditionMethod!.MakeGenericMethod(entityType);
        var expressionType = typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(entityType, typeof(bool)));
        var compileMethod = expressionType.GetMethod(nameof(LambdaExpression.Compile), Array.Empty<Type>())!;
        return arguments => compileMethod.Invoke(genericConditionMethod.Invoke(conditionBuilder, arguments), Array.Empty<object>()) ?? throw new InvalidOperationException();
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
    
    protected Type GetEntityType(string entityKey, ScenarioDomainModel model)
    {
        var entityModel = model.Entities.SingleOrDefault(e => e.Value == entityKey)
                          ?? throw new ArgumentException($"No entity matches key: {entityKey}");

        var entityType = domainTypeResolver.ResolveType(entityModel.Value)
                         ?? throw new ArgumentException($"Cannot find type for {entityModel.Label}");
        return entityType!;
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

    public DatabaseScenario Map(ScenarioFlow flow)
    {
        return new DatabaseScenario
        {
            Id = Guid.Parse(flow.Key),
            // TODO
        };
    }
}