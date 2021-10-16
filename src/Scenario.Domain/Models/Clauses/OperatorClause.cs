namespace Scenario.Domain.Clauses
{
    public abstract class OperatorClause : IPredicateClause
    {
        public OperatorClause(string operatorKey)
        {
            OperatorKey = operatorKey;
        }

        public string OperatorKey { get; set; }

        public abstract string Discriminator { get; }
    }
}
