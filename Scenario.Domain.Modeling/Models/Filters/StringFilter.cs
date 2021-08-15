using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenario.Domain.Modeling.Models.Filters
{
    public class StringFilter : BaseFilter<string, string>
    {
        public string Label { get; set; }
        public string Value { get; set; }

        public override IDictionary<string, Func<string, string, bool>> SupportedComparisons => new Dictionary<string, Func<string, string, bool>>
        {
            { "less than", (t1, t2) => t1?.CompareTo(t2) < 0 },
            { "less than or equal to", (t1, t2) => t1?.CompareTo(t2) <= 0 },
            { "equal to", (t1, t2) => t1 == t2 },
            { "not equal to", (t1, t2) => t1 != t2},
            { "greater than", (t1, t2) => t1?.CompareTo(t2) > 0 },
            { "greater than or equal to", (t1, t2) => t1?.CompareTo(t2) >= 0 },
            { "contains", (t1, t2) => t1?.Contains(t2) ?? false },
            { "not contains", (t1, t2) => !t1?.Contains(t2) ?? false },
            { "starts with", (t1, t2) => t1?.StartsWith(t2) ?? false },
            { "not starts with", (t1, t2) => !t1?.StartsWith(t2) ?? false },
            { "ends with", (t1, t2) => t1?.EndsWith(t2) ?? false },
            { "not ends with", (t1, t2) => !t1?.EndsWith(t2) ?? false },
            { "is empty", (t1, t2) => string.IsNullOrEmpty(t1) },
            { "is not empty", (t1, t2) => string.IsNullOrEmpty(t1) },
        };

        public override IList<string> SupportedComparisonsKeys => SupportedComparisons.Keys.ToList();
    }
}
