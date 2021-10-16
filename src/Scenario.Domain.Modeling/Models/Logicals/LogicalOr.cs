using System;
namespace Scenario.Domain.Modeling.Models.Logicals
{
    public class LogicalOr : ILogical
    {
        public LogicalOr()
        {
        }

        public string Key => "LOGICAL_OR";

        public bool Apply(bool value1, bool value2)
        {
            return value1 || value2;
        }
    }
}
