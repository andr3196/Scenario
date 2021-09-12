namespace Scenario.Domain.Clauses
{
    public abstract class OperatorClause : IPredicateClause
    {
        public OperatorClause(string operatorValue)
        {
            Operator = operatorValue;
        }

        public string Operator { get; set; }

        public abstract string Discriminator { get; }
    }
}
