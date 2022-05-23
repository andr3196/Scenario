using System;
using System.Linq.Expressions;
using Scenario.Domain.Models.Clauses;

namespace Scenario.Domain.Services.ExpressionBuilding
{
    public interface IPredicateClauseExpressionBuilder
    {
        Expression<Func<TInput, bool>> GetPredicateExpression<TInput>(IPredicateClause predicateClause, ParameterExpression parameter);
    }
}
