using System;
using Scenario.Domain.Modeling.Models.Constants;

namespace Scenario.Test.Services.ExpressionBuilding.Mocks
{
    public class CurrentUserIdMock : IConstant<Guid>
    {
        public Guid Value => Guid.Parse("f0ae1659-7082-4b3a-a9a3-2911e9862d66");

        public string Key => "CURRENT_USERID";

        public object Evaluate()
        {
            return Value;
        }
    }
}
