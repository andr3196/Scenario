namespace Scenario.Domain.Models;

public class ScenarioDefinition
{
    public ScenarioDefinition(Guid id)
    {
        Id = id;
    }

    public ScenarioDefinition()
    {
        Id = Guid.NewGuid();
    }
    
    public Guid Id { get; }
    
    public string? Title { get; set; }

    public string ConditionJson { get; set; }
    
    public string ConsequenceJson { get; set; }
    
    public bool IsActive { get; set; }
}