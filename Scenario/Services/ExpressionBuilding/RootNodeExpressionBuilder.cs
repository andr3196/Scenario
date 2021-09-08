using System;
using System.Linq.Expressions;
using Scenario.Domain.Clauses;

namespace Scenario.Services.ExpressionBuilding
{
    public class RootNodeExpressionBuilder : IScenarioConditionExpressionBuilder
    {
        private readonly IPredicateClauseExpressionBuilder predicateClauseExpressionBuilder;

        public RootNodeExpressionBuilder(IPredicateClauseExpressionBuilder predicateClauseExpressionBuilder)
        {
            this.predicateClauseExpressionBuilder = predicateClauseExpressionBuilder;
        }

        public Expression<Func<TInput, bool>> BuildExpression<TInput>(RootNodeWhereClause conditionClause, Type entityType)
        {
            var entityParameter = Expression.Parameter(entityType);
            return predicateClauseExpressionBuilder.GetPredicateExpression<TInput>(conditionClause.Value, entityParameter);
        }
    }
}
