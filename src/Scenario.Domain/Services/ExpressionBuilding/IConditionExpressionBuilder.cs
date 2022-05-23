using System;
using System.Linq.Expressions;
using Scenario.Domain.Models.Clauses;

namespace Scenario.Domain.Services.ExpressionBuilding
{
    public interface IConditionExpressionBuilder
    {
        Expression<Func<TInput, bool>> BuildExpression<TInput>(IPredicateClause clause, Type entityType);
    }
}
