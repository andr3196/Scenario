using System;
using System.Linq.Expressions;

namespace Scenario.Domain.Clauses
{
    public class UnaryFilterWhereClause : FilterWhereClause
    {
        public ValueWhereClause Value { get; set; }

        public override string Discriminator => nameof(UnaryFilterWhereClause);

        public override Expression<Func<object, bool>> GetPredicateExpression(ParameterExpression parameter)
        {
           Expression body;
           if (Operator == "isNotNull")
           {
                body = Expression.NotEqual(Value.GetExpression(parameter), Expression.Constant(null));
           } else
           {
                body = Expression.Equal(Value.GetExpression(parameter), Expression.Constant(null));
           }
           return Expression.Lambda<Func<object, bool>>(body, parameter);
        }
    }
}
