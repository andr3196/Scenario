using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Scenario.Domain.Clauses;
using Scenario.Domain.Modeling.Models.Constants;
using Scenario.Domain.Modeling.Models.Filters;
using Scenario.Domain.SharedTypes;
using Scenario.ErrorHandling;

namespace Scenario.Services.ExpressionBuilding
{
    public class PredicateClauseExpressionBuilder : IPredicateClauseExpressionBuilder
    {
        private readonly IEnumerable<IFilter> filters;
        private readonly IValueClauseExpressionBuilder valueClauseExpressionBuilder;
        private readonly IDomainTypeResolver domainTypeResolver;

        public PredicateClauseExpressionBuilder(IEnumerable<IFilter> filters, IValueClauseExpressionBuilder valueClauseExpressionBuilder, IDomainTypeResolver domainTypeResolver)
        {
            this.filters = filters;
            this.valueClauseExpressionBuilder = valueClauseExpressionBuilder;
            this.domainTypeResolver = domainTypeResolver;
        }

        public Expression<Func<TInput, bool>> GetPredicateExpression<TInput>(IPredicateWhereClause predicateWhereClause, ParameterExpression parameter)
        {
            return GetExpression<TInput>(predicateWhereClause, parameter);
        }

        protected Expression<Func<TInput, bool>> GetExpression<TInput>(IPredicateWhereClause clause, ParameterExpression parameter)
        {
            return clause switch
            {
                UnaryFilterWhereClause unaryClause => GetExpression<TInput>(unaryClause, parameter),
                BinaryFilterWhereClause binaryClause => GetExpression<TInput>(binaryClause, parameter),
                UnaryLogicalWhereClause unaryLogical => GetExpression<TInput>(unaryLogical, parameter),
                BinaryLogicalWhereClause binaryLogical => GetExpression<TInput>(binaryLogical, parameter),
                _ => throw new NotSupportedException($"Clause of type {clause.GetType().Name} is not supported")
            };
        }

        protected Expression<Func<TInput, bool>> GetExpression<TInput>(BinaryFilterWhereClause clause, ParameterExpression parameter)
        {
            var leftType = domainTypeResolver.ResolveTypeFromKey(clause.Left.ValueType) ?? throw new ScenarioExpressionBuilderException();
            var rightType = domainTypeResolver.ResolveTypeFromKey(clause.Right.ValueType) ?? throw new ScenarioExpressionBuilderException();

            var filterType = typeof(IFilter<,>).MakeGenericType(new[] { leftType, rightType });
            var filter = filters.SingleOrDefault(f => filterType.IsAssignableFrom(f.GetType()) && f.SupportedComparisonsKeys.Contains(clause.Operator));

            if (filter == null)
            {
                throw new ArgumentException($"No filter supports the requested operation '{leftType.Name}' {clause.Operator} '{rightType.Name}'");
            }
            if (!filter.SupportedComparisonsKeys.Contains(clause.Operator))
            {
                throw new ArgumentOutOfRangeException($"The operator {clause.Operator} is not supported by filter of type {filter.GetType()}");
            }
            Expression<Func<object, object, bool>> filterAction = (left, right) => filter.Compare(left, right, clause.Operator);

            var leftExpression = valueClauseExpressionBuilder.GetExpression(clause.Left, parameter);
            var rightExpression = valueClauseExpressionBuilder.GetExpression(clause.Right, parameter);

            var arguments = new Expression[] {
                Expression.Convert(leftExpression, typeof(object)),
                Expression.Convert(rightExpression, typeof(object)),
                Expression.Constant(clause.Operator) };
            var body = Expression.Call(Expression.Constant(filter), typeof(IFilter).GetMethod(nameof(IFilter.Compare))!, arguments);
            return Expression.Lambda<Func<TInput, bool>>(body, parameter);
        }

        protected Expression<Func<TInput, bool>> GetExpression<TInput>(UnaryFilterWhereClause clause, ParameterExpression parameter)
        {
            Expression body;

            var valueExpression = valueClauseExpressionBuilder.GetExpression(clause.Value, parameter);
            if (clause.Operator == "isNotNull")
            {
                body = Expression.NotEqual(valueExpression, Expression.Constant(null));
            }
            else
            {
                body = Expression.Equal(valueExpression, Expression.Constant(null));
            }
            return Expression.Lambda<Func<TInput, bool>>(body, parameter);
        }

        protected Expression<Func<TInput, bool>> GetExpression<TInput>(UnaryLogicalWhereClause clause, ParameterExpression parameter)
        {
            return GetExpression<TInput>(clause.Value, parameter);
        }

        protected Expression<Func<TInput, bool>> GetExpression<TInput>(BinaryLogicalWhereClause clause, ParameterExpression parameter)
        {
            var leftFunctionExpression = GetExpression<TInput>(clause.Left, parameter);
            var rightFunctionExpression = GetExpression<TInput>(clause.Right, parameter);

            var leftExpression = Expression.Invoke(leftFunctionExpression, parameter);
            var rightExpression = Expression.Invoke(rightFunctionExpression, parameter);

            Expression body;

            if (clause.Combinator == "LOGICAL_AND")
            {
                body = Expression.AndAlso(leftExpression, rightExpression);
            }
            else
            {
                body = Expression.OrElse(leftExpression, rightExpression);
            }

            return Expression.Lambda<Func<TInput, bool>>(body, parameter);
        }
    }
}
