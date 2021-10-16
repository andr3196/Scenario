using System;
using System.Collections.Generic;
using Scenario.Domain.Modeling.Models.Filters;

namespace Scenario.Test.Services.ExpressionBuilding.Mocks.Filters
{
    public class GenericUnaryFilterMock<TInput> : UnaryBaseFilter<TInput>
    {
        private readonly Dictionary<string, Func<TInput, bool>> operations;

        public GenericUnaryFilterMock(Dictionary<string, Func<TInput, bool>> operations)
        {
            this.operations = operations;
        }

        public override IDictionary<string, Func<TInput, bool>> SupportedComparisons => operations;
    }
}
