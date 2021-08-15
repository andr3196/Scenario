using System;
using System.Linq.Expressions;

namespace Scenario.Domain.Clauses
{
    public class BinaryLogicalWhereClause : IPredicateWhereClause
    {
        public FilterWhereClause Left { get; set; }

        public FilterWhereClause Right { get; set; }

        public string Combinator { get; set; }

        public string Discriminator => nameof(BinaryLogicalWhereClause);

        public Expression<Func<object, bool>> GetPredicateExpression(ParameterExpression parameter)
        {
            var leftExpression = Left.GetPredicateExpression(parameter);
            var rightExpression = Right.GetPredicateExpression(parameter);

            Expression body;

            if (Combinator == "LOGICAL_AND")
            {
                body = Expression.AndAlso(leftExpression, rightExpression);
            }
            else
            {
                body = Expression.OrElse(leftExpression, rightExpression);
            }

            return Expression.Lambda<Func<object, bool>>(body, parameter);
        }
    }
}
