using System;
using System.Collections.Generic;
using Scenario.Domain.Modeling.Models.Filters;

namespace Scenario.Test.Services.ExpressionBuilding.Mocks.Filters
{
    public class GenericBinaryFilterMock<TInput1, TInput2> : BaseFilter<TInput1, TInput2>
    {
        private readonly Dictionary<string, Func<TInput1, TInput2, bool>> operations;

        public GenericBinaryFilterMock(Dictionary<string, Func<TInput1, TInput2, bool>> operations)
        {
            this.operations = operations;
        }

        public override IDictionary<string, Func<TInput1, TInput2, bool>> SupportedComparisons => operations;
    }
}
