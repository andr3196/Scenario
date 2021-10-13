using System;
using System.Collections.Generic;
using System.Linq;

namespace Scenario.Domain.Modeling.Models.Filters
{
    public abstract class UnaryBaseFilter<T1> : IFilter<T1>
    {
        public abstract IDictionary<string, Func<T1, bool>> SupportedComparisons { get; }

        public virtual IList<string> SupportedComparisonsKeys => SupportedComparisons.Keys.ToList();

        public bool Compare(T1 t1, string comparisonType)
        {

            if (SupportedComparisons.Keys.Contains(comparisonType))
            {
                return SupportedComparisons[comparisonType](t1);
            }

            throw new ArgumentOutOfRangeException($"The value '{comparisonType}' is not a valid number comparison");
        }

        public bool Compare(object obj1, object obj2, string comparisonType)
        {
            if (obj1 is T1 t1)
            {
                return this.Compare(t1, comparisonType);
            }
            throw new ArgumentException($"Arguments of types {obj1.GetType()} and {obj2?.GetType()} are not valid for comparison by a filter of type {this.GetType()}");
        }
    }
}
