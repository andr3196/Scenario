using System.Text.Json.Serialization;

namespace Scenario.Domain.Models.Clauses
{
    public class UnaryOperatorClause : OperatorClause
    {
        public UnaryOperatorClause(ValueClause value, string operatorKey)
            : base(operatorKey)
        {
            Value = value;
        }

        public ValueClause Value { get; set; }
        
        [JsonPropertyOrder(-1)]
        public override string Discriminator => nameof(UnaryOperatorClause);
    }
}
