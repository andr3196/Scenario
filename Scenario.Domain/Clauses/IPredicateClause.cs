namespace Scenario.Domain.Clauses
{
    public interface IPredicateClause
    {
        public string Discriminator { get; }
    }
}
