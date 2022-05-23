using System.Text.Json;
using Scenario.Domain.Models;
using Scenario.Domain.Models.Clauses;

namespace Scenario.TestDataBuilder.Domain;

public class ScenarioDefinitionBuilder : DataBuilder<ScenarioDefinition>
{
    public ScenarioDefinitionBuilder WithId(string title)
    {
        return With(s => s.Title = title);
        
    }
    
    public ScenarioDefinitionBuilder WithTitle(string title)
    {
        return With(s => s.Title = title);
        
    }
    
    public ScenarioDefinitionBuilder IsActive(bool active = true)
    {
        return With(s => s.IsActive = active);
    }
    
    public ScenarioDefinitionBuilder WithDummyCondition()
    {
        return WithCondition(new UnaryPredicateClause(new UnaryOperatorClause(new ValueClause("type", "valueType", "value"), "operator")));
    }
    
    public ScenarioDefinitionBuilder WithCondition(IPredicateClause conditionClause)
    {
        return With(s => s.ConditionJson = JsonSerializer.Serialize(conditionClause));
    }
    
    

    public ScenarioDefinitionBuilder WithDummyConsequence()
    {
        return With(s => s.ConsequenceJson = JsonSerializer.Serialize(new ConsequenceClause("consequence-1", new Dictionary<string, ValueClause>())));
    }

    public ScenarioDefinitionBuilder Repeat()
    {
        var currentAssignments = CurrentAssignments;
        Next();
        AddRange(currentAssignments);
        return this;
    }
    
    public new ScenarioDefinitionBuilder With(Action<ScenarioDefinition> assignment)
    {
        base.With(assignment);
        return this;
    }

    protected override ScenarioDefinition Construct()
    {
        return new ScenarioDefinition(Guid.NewGuid());
    }
}