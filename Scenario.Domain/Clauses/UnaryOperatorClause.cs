namespace Scenario.Domain.Clauses
{
    public class UnaryOperatorClause : OperatorClause
    {
        public UnaryOperatorClause(string @operator, ValueClause value)
            : base(@operator)
        {
            Value = value;
        }

        public ValueClause Value { get; set; }

        public override string Discriminator => nameof(UnaryOperatorClause);
    }
}
