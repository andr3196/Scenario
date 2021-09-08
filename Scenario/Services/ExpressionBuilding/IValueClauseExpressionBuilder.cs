using System;
using System.Linq.Expressions;
using Scenario.Domain.Clauses;

namespace Scenario.Services.ExpressionBuilding
{
    public interface IValueClauseExpressionBuilder
    {
        Expression GetExpression(ValueWhereClause clause, ParameterExpression parameter);
    }
}
