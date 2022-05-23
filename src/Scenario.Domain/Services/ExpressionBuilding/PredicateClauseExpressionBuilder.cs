using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Scenario.Domain.Exceptions;
using Scenario.Domain.Modeling.Models.Filters;
using Scenario.Domain.Modeling.Models.Logicals;
using Scenario.Domain.Models.Clauses;
using Scenario.Domain.Shared.TypeHandling;

namespace Scenario.Domain.Services.ExpressionBuilding
{
    public class PredicateClauseExpressionBuilder : IPredicateClauseExpressionBuilder
    {
        private readonly IEnumerable<IFilter> filters;
        private readonly IEnumerable<ILogical> logicals;
        private readonly IValueClauseExpressionBuilder valueClauseExpressionBuilder;
        private readonly IDomainTypeResolver domainTypeResolver;

        public PredicateClauseExpressionBuilder(IEnumerable<IFilter> filters, IEnumerable<ILogical> logicals, IValueClauseExpressionBuilder valueClauseExpressionBuilder, IDomainTypeResolver domainTypeResolver)
        {
            this.filters = filters;
            this.logicals = logicals;
            this.valueClauseExpressionBuilder = valueClauseExpressionBuilder;
            this.domainTypeResolver = domainTypeResolver;
        }

        public Expression<Func<TInput, bool>> GetPredicateExpression<TInput>(IPredicateClause predicateClause, ParameterExpression parameter)
        {
            return GetExpression<TInput>(predicateClause, parameter);
        }

        protected Expression<Func<TInput, bool>> GetExpression<TInput>(IPredicateClause clause, ParameterExpression parameter)
        {
            return clause switch
            {
                UnaryOperatorClause unaryClause => GetExpression<TInput>(unaryClause, parameter),
                BinaryOperatorClause binaryClause => GetExpression<TInput>(binaryClause, parameter),
                UnaryPredicateClause unaryLogical => GetExpression<TInput>(unaryLogical, parameter),
                BinaryPredicateClause binaryLogical => GetExpression<TInput>(binaryLogical, parameter),
                _ => throw new NotSupportedException($"Clause of type {clause.Discriminator} is not supported")
            };
        }

        protected Expression<Func<TInput, bool>> GetExpression<TInput>(BinaryOperatorClause clause, ParameterExpression parameter)
        {
            var leftType = domainTypeResolver.ResolveType(clause.Left.ValueType)
                ?? throw new ExpressionBuildingException($"Failed to resolve left node with valueType: {clause.Left.ValueType}");
            var rightType = domainTypeResolver.ResolveType(clause.Right.ValueType)
                ?? throw new ExpressionBuildingException($"Failed to resolve left node with valueType: {clause.Left.ValueType}");

            var filterType = typeof(IFilter<,>).MakeGenericType(new[] { leftType, rightType });
            var filter = filters.SingleOrDefault(f => filterType.IsAssignableFrom(f.GetType()) && f.SupportedComparisonsKeys.Contains(clause.OperatorKey))
                ?? throw new ArgumentException($"No filter supports the requested operation '{leftType.Name}' {clause.OperatorKey} '{rightType.Name}'");

            var leftExpression = valueClauseExpressionBuilder.GetExpression(clause.Left, parameter);
            var rightExpression = valueClauseExpressionBuilder.GetExpression(clause.Right, parameter);

            var arguments = new Expression[] {
                Expression.Convert(Expression.Invoke(leftExpression, parameter), typeof(object)),
                Expression.Convert(Expression.Invoke(rightExpression, parameter), typeof(object)),
                Expression.Constant(clause.OperatorKey) };
            var body = Expression.Call(Expression.Constant(filter), typeof(IFilter).GetMethod(nameof(IFilter.Compare))!, arguments);
            return Expression.Lambda<Func<TInput, bool>>(body, parameter);
        }

        protected Expression<Func<TInput, bool>> GetExpression<TInput>(UnaryOperatorClause clause, ParameterExpression parameter)
        {
            var valueType = domainTypeResolver.ResolveType(clause.Value.ValueType)
                ?? throw new ExpressionBuildingException($"Failed to resolve node with valueType: {clause.Value.ValueType}");

            var filterType = typeof(IFilter<>).MakeGenericType(new[] { valueType });
            var filter = filters.SingleOrDefault(f => filterType.IsAssignableFrom(f.GetType()) && f.SupportedComparisonsKeys.Contains(clause.OperatorKey))
                ?? throw new ArgumentException($"No filter supports the requested operation '{valueType.Name}' {clause.OperatorKey}");

            var valueExpression = valueClauseExpressionBuilder.GetExpression(clause.Value, parameter);

            var arguments = new Expression[] {
                Expression.Convert(Expression.Invoke(valueExpression, parameter), typeof(object)),
                Expression.Constant(null),
                Expression.Constant(clause.OperatorKey) };
            var body = Expression.Call(Expression.Constant(filter), typeof(IFilter).GetMethod(nameof(IFilter.Compare))!, arguments);
            return Expression.Lambda<Func<TInput, bool>>(body, parameter);
        }

        protected Expression<Func<TInput, bool>> GetExpression<TInput>(UnaryPredicateClause clause, ParameterExpression parameter)
        {
            return GetExpression<TInput>(clause.Value, parameter);
        }

        protected Expression<Func<TInput, bool>> GetExpression<TInput>(BinaryPredicateClause clause, ParameterExpression parameter)
        {
            var leftFunctionExpression = GetExpression<TInput>(clause.Left, parameter);
            var rightFunctionExpression = GetExpression<TInput>(clause.Right, parameter);

            var leftExpression = Expression.Invoke(leftFunctionExpression, parameter);
            var rightExpression = Expression.Invoke(rightFunctionExpression, parameter);

            
            var logicalOperator = logicals.SingleOrDefault(l => l.Key == clause.Combinator)
                ?? throw new ArgumentException($"Failed to find logical for combinator key {clause.Combinator}");
            var applyMethod = logicalOperator.GetType().GetMethod(nameof(ILogical.Apply))!;
            var body = Expression.Call(
                Expression.Constant(logicalOperator),
                applyMethod,
                leftExpression,
                rightExpression);
            return Expression.Lambda<Func<TInput, bool>>(body, parameter);
        }
    }
}
