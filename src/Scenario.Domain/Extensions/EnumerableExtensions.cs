using System.Collections.Generic;
using System.Linq;

namespace Scenario.Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> AppendIf<T>(this IEnumerable<T> source, T value,  bool shouldAppend){
            return shouldAppend ? source.Append(value) : source;
        }
    }
}