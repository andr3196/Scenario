using System;
using System.Linq.Expressions;

namespace Scenario.Domain.Clauses
{
    public class BinaryLogicalWhereClause : IPredicateWhereClause
    {
        public IPredicateWhereClause Left { get; set; }

        public IPredicateWhereClause Right { get; set; }

        public string Combinator { get; set; }

        public string Discriminator => nameof(BinaryLogicalWhereClause);
    }
}
