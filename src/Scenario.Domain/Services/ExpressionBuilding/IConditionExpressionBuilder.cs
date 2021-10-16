using System;
using System.Linq.Expressions;
using Scenario.Domain.Clauses;

namespace Scenario.Services.ExpressionBuilding
{
    public interface IConditionExpressionBuilder
    {
        Expression<Func<TInput, bool>> BuildExpression<TInput>(IPredicateClause clause, Type entityType);
    }
}
