using Scenario.Domain.Models.Clauses;

namespace Scenario.Domain.Serialization.JsonConvertion
{
    public interface IConsequenceClauseConverter
    {
        public string Serialize(ConsequenceClause clause);
        
        public ConsequenceClause Deserialize(string clauseJson);
    }
}