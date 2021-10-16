using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenario.Domain.Modeling.Models.Filters
{
    public class NumberFilter : BaseFilter<double?, double?>
    {
        public string Label { get; set; }
        public string Value { get; set; }

        public override IDictionary<string, Func<double?, double?, bool>> SupportedComparisons => new Dictionary<string, Func<double?, double?, bool>>
        {
            { "less than", (t1, t2) => t1?.CompareTo(t2) < 0 },
            { "less than or equal to", (t1, t2) => t1?.CompareTo(t2) <= 0 },
            { "equal to", (t1, t2) => t1 == t2 },
            { "not equal to", (t1, t2) => t1 != t2},
            { "greater than", (t1, t2) => t1?.CompareTo(t2) > 0 },
            { "greater than or equal to", (t1, t2) => t1?.CompareTo(t2) >= 0 },
            { "is empty", (t1, t2) => !t1.HasValue },
            { "is not empty", (t1, t2) => t1.HasValue },
        };

        public override IList<string> SupportedComparisonsKeys => SupportedComparisons.Keys.ToList();
    }
}
