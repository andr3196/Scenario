using Scenario.Domain.Models.Clauses;

namespace Scenario.Contracts
{
    public record ScenarioDefinitionDto
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; }

        public bool IsActive { get; set; }
        
        public IPredicateClause Condition { get; set; }

        public ConsequenceClause Consequence { get; set; }
        
        public string Event { get; set; }
        
        public string Entity { get; set; }
    }
    
    public record ScenarioMetadataDto
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; }

        public bool IsActive { get; set; }
    }
    
    public record ScenarioDetails : ScenarioMetadataDto
    {
        public IPredicateClause Condition { get; set; }

        public ConsequenceClause Consequence { get; set; }
    }
    
}
