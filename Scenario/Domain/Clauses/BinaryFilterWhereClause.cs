using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Scenario.Domain.Modeling.Models.Filters;

namespace Scenario.Domain.Clauses
{
    public class BinaryFilterWhereClause : FilterWhereClause
    {
        private readonly IEnumerable<IFilter> filters;

        public BinaryFilterWhereClause(IEnumerable<IFilter> filters)
        {
            this.filters = filters;
        }

        public ValueWhereClause Left { get; set; }

        public ValueWhereClause Right { get; set; }

        public override string Discriminator => nameof(BinaryFilterWhereClause);

        public override Expression<Func<object, bool>> GetPredicateExpression(ParameterExpression parameter)
        {
            var filterType = typeof(IFilter<,>).MakeGenericType(new[] { Left.ValueType, Right.ValueType });
            var filter = filters.SingleOrDefault(f => filterType.IsAssignableFrom(f.GetType()) && f.SupportedComparisonsKeys.Contains(Operator));

            if(filter == null)
            {
                throw new ArgumentException($"No filter supports the requested operation '{Left.ValueType.Name}' {Operator} '{Right.ValueType.Name}'");
            }
            if(!filter.SupportedComparisonsKeys.Contains(Operator))
            {
                throw new ArgumentOutOfRangeException($"The operator {Operator} is not supported by filter of type {filter.GetType()}");
            }
            Expression<Func<object, object, bool>> filterAction = (left, right) => filter.Compare(left, right, Operator);

            var leftExpression = Left.GetExpression(parameter);
            var rightExpression = Right.GetExpression(parameter);
            var invocation = Expression.Invoke(filterAction, new[] { leftExpression, rightExpression });
           return Expression.Lambda<Func<object, bool>>(invocation, parameter);
        }
    }
}
