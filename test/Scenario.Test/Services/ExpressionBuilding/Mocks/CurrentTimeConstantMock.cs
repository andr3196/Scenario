using System;
using Scenario.Domain.Modeling.Models.Constants;

namespace Scenario.Test.Services.ExpressionBuilding.Mocks
{
    public class CurrentTimeConstantMock : IConstant<DateTime>
    {
        public DateTime Value => default(DateTime).AddDays(7);

        public string Key => "CURRENT_TIME";

        public object Evaluate()
        {
            return Value;
        }
    }
}
