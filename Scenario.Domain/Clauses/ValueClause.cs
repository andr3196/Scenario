namespace Scenario.Domain.Clauses
{
    public class ValueClause
    {
        public ValueClause(string type, string valueType, string value)
        {
            Type = type;
            ValueType = valueType;
            Value = value;
        }

        public string Type { get; set; }

        public string ValueType { get; set; }

        public string Value { get; set; }

    }
}
