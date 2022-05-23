using Scenario.Domain.Models.Clauses;

namespace Scenario.Domain.Serialization.JsonConvertion
{
    public interface IPredicateClauseConverter
    {
        public string Serialize(IPredicateClause clause);
        
        public IPredicateClause Deserialize(string clauseJson);
    }
}