namespace Scenario.Domain.Models.Clauses
{
    public class BinaryPredicateClause : IPredicateClause
    {
        public BinaryPredicateClause(IPredicateClause left, IPredicateClause right, string combinator)
        {
            Combinator = combinator;
            Left = left;
            Right = right;
        }

        public IPredicateClause Left { get; set; }

        public IPredicateClause Right { get; set; }

        public string Combinator { get; set; }

        public string Discriminator => nameof(BinaryPredicateClause);
    }
}
