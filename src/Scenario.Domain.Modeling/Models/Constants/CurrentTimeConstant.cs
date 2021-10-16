using System;
namespace Scenario.Domain.Modeling.Models.Constants
{
    public class CurrentTimeConstant : IConstant<DateTime>
    {
        public CurrentTimeConstant()
        {
        }

        public string Key => "CURRENT_TIME";

        public DateTime Value => DateTime.Now;

        public object Evaluate()
        {
            return Value;
        }
    }
}
