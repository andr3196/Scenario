using System;
using System.Linq.Expressions;
using Scenario.Domain.Clauses;

namespace Scenario.Services.ExpressionBuilding
{
    public interface IPredicateClauseExpressionBuilder
    {
        Expression<Func<TInput, bool>> GetPredicateExpression<TInput>(IPredicateClause predicateClause, ParameterExpression parameter);
    }
}
