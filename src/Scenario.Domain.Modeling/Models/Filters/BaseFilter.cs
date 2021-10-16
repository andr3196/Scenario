using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenario.Domain.Modeling.Models.Filters
{
    public abstract class BaseFilter<T1, T2> : IFilter<T1, T2>
    {
        public abstract IDictionary<string, Func<T1, T2, bool>> SupportedComparisons { get; }

        public virtual IList<string> SupportedComparisonsKeys => SupportedComparisons.Keys.ToList();

        public bool Compare(T1 t1, T2 t2, string comparisonType)
        {

            if (SupportedComparisons.Keys.Contains(comparisonType))
            {
                return SupportedComparisons[comparisonType](t1, t2);
            }

            throw new ArgumentOutOfRangeException($"The value '{comparisonType}' is not a valid number comparison");
        }

        public bool Compare(object obj1, object obj2, string comparisonType)
        {
            if (obj1 is T1 t1 && obj2 is T2 t2)
            {
                return this.Compare(t1, t2, comparisonType);
            }
            throw new ArgumentException($"Arguments of types {obj1.GetType()} and {obj2?.GetType()} are not valid for comparison by a filter of type {this.GetType()}");
        }
    }
}
