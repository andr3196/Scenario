using System;
using System.Linq.Expressions;

namespace Scenario.Domain.Clauses
{
    public class UnaryFilterWhereClause : FilterWhereClause
    {
        public ValueWhereClause Value { get; set; }

        public override string Discriminator => nameof(UnaryFilterWhereClause);
    }
}
