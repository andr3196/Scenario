namespace Scenario.Domain.Clauses
{
    public abstract class FilterWhereClause : IPredicateWhereClause
    {
        public string Operator { get; set; }

        public abstract string Discriminator { get; }
    }
}
