using System;
using System.Collections.Generic;

namespace Scenario.Domain.Modeling.Models.Filters
{
    public interface IFilter<T1,T2> : IFilter
    {
        public bool Compare(T1 t1, T2 t2, string comparisonType);

        public IDictionary<string, Func<T1, T2, bool>> SupportedComparisons { get; }
    }

    public interface IFilter
    {
        public IList<string> SupportedComparisonsKeys { get; }

        public bool Compare(object obj1, object obj2, string comparisonType);
    } 
}
