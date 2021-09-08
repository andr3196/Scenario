using System;
using System.Linq.Expressions;

namespace Scenario.Domain.Clauses
{
    public class UnaryLogicalWhereClause : IPredicateWhereClause
    {
        public IPredicateWhereClause Value { get; set; }

        public string Discriminator => nameof(UnaryLogicalWhereClause);
    }
}
