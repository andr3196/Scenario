using System;
using System.Collections.Generic;
using Scenario.Domain.Clauses;

namespace Scenario.Services
{
    public class SingletonCacheService
    {
        public Dictionary<string, Type> PredicateTypeLookupTable => new Dictionary<string, Type>
        {
            {nameof(BinaryFilterWhereClause), typeof(BinaryFilterWhereClause) },
            {nameof(UnaryFilterWhereClause), typeof(UnaryFilterWhereClause) },
            {nameof(BinaryLogicalWhereClause), typeof(BinaryLogicalWhereClause) },
            {nameof(UnaryLogicalWhereClause), typeof(UnaryLogicalWhereClause) },

        };
    }
}
