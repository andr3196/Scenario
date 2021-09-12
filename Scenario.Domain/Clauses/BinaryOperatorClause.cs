namespace Scenario.Domain.Clauses
{
    public class BinaryOperatorClause : OperatorClause
    {
        public BinaryOperatorClause(ValueClause left, ValueClause right, string @operator)
            : base(@operator)
        {
            Left = left;
            Right = right;
        }

        public ValueClause Left { get; set; }

        public ValueClause Right { get; set; }

        public override string Discriminator => nameof(BinaryOperatorClause);
    }
}
