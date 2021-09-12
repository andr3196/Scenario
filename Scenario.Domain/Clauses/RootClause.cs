

namespace Scenario.Domain.Clauses
{
    public class RootClause
    {
        public RootClause(IPredicateClause value)
        {
            Value = value;
        }

        public IPredicateClause Value { get; set; }

    }
}
