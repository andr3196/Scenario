using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenario.Domain.Modeling.Models.Filters
{
    public class DateTimeFilter : BaseFilter<DateTime, DateTime>
    {
        public string Label { get; set; }
        public string Value { get; set; }

        public override IDictionary<string, Func<DateTime, DateTime, bool>> SupportedComparisons => new Dictionary<string, Func<DateTime, DateTime, bool>>
        {
            { "before", (t1, t2) => t1 < t2 },
            { "before or at", (t1, t2) => t1 <= t2 },
            { "at", (t1, t2) => t1 == t2 },
            { "not at", (t1, t2) => t1 != t2},
            { "after", (t1, t2) => t1 > t2 },
            { "after or at", (t1, t2) => t1 >= t2 },
        };

        public override IList<string> SupportedComparisonsKeys => SupportedComparisons.Keys.ToList();
    }
}
