using System;
using System.Linq.Expressions;
using Scenario.Domain.Models.Clauses;

namespace Scenario.Domain.Services.ExpressionBuilding
{
    public class ConditionExpressionBuilder : IConditionExpressionBuilder
    {
        private readonly IPredicateClauseExpressionBuilder predicateClauseExpressionBuilder;

        public ConditionExpressionBuilder(IPredicateClauseExpressionBuilder predicateClauseExpressionBuilder)
        {
            this.predicateClauseExpressionBuilder = predicateClauseExpressionBuilder;
        }

        public Expression<Func<TInput, bool>> BuildExpression<TInput>(IPredicateClause clause, Type entityType)
        {
            var parameter = Expression.Parameter(entityType);
            return predicateClauseExpressionBuilder.GetPredicateExpression<TInput>(clause, parameter);
        }
    }
}
