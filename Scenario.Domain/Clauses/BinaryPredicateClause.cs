namespace Scenario.Domain.Clauses
{
    public class BinaryPredicateClause : IPredicateClause
    {
        public BinaryPredicateClause(string combinator, IPredicateClause leftClause, IPredicateClause rightClause)
        {
            Combinator = combinator;
            Left = leftClause;
            Right = rightClause;
        }

        public IPredicateClause Left { get; set; }

        public IPredicateClause Right { get; set; }

        public string Combinator { get; set; }

        public string Discriminator => nameof(BinaryPredicateClause);
    }
}
