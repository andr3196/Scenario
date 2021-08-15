using System;
using System.Linq.Expressions;

namespace Scenario.Domain.Clauses
{
    public class UnaryLogicalWhereClause : IPredicateWhereClause
    {
        public FilterWhereClause Value { get; set; }

        public string Discriminator => nameof(UnaryLogicalWhereClause);

        public Expression<Func<object, bool>> GetPredicateExpression(ParameterExpression parameter)
        {
            return Value.GetPredicateExpression(parameter);
        }
    }
}
