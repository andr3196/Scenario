namespace Scenario.Domain.Modeling.Models.Logicals
{
    public class LogicalAnd : ILogical
    {
        public LogicalAnd()
        {
        }

        public string Key => "LOGICAL_AND";

        public bool Apply(bool value1, bool value2)
        {
            return value1 && value2;
        }
    }
}
