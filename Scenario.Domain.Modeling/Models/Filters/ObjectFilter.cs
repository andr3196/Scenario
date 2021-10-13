using System;
using System.Collections.Generic;

namespace Scenario.Domain.Modeling.Models.Filters
{
    public class ObjectFilter : UnaryBaseFilter<object>
    {
        public override IDictionary<string, Func<object, bool>> SupportedComparisons => new Dictionary<string, Func<object, bool>>
        {
            { "is empty", t1 => t1 == null },
            { "is not empty", t1 => t1 != null },
        };
    }
}
