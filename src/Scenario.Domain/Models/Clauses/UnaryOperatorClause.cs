namespace Scenario.Domain.Clauses
{
    public class UnaryOperatorClause : OperatorClause
    {
        public UnaryOperatorClause(ValueClause value, string operatorKey)
            : base(operatorKey)
        {
            Value = value;
        }

        public ValueClause Value { get; set; }

        public override string Discriminator => nameof(UnaryOperatorClause);
    }
}
