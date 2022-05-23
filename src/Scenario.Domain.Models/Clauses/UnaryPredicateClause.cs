namespace Scenario.Domain.Models.Clauses
{
    public class UnaryPredicateClause : IPredicateClause
    {
        public UnaryPredicateClause(IPredicateClause value)
        {
            Value = value;
        }

        public IPredicateClause Value { get; set; }

        public string Discriminator => nameof(UnaryPredicateClause);
    }
}
