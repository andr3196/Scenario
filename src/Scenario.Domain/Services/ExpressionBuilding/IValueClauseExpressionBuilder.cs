using System.Linq.Expressions;
using Scenario.Domain.Clauses;

namespace Scenario.Services.ExpressionBuilding
{
    public interface IValueClauseExpressionBuilder
    {
        LambdaExpression GetExpression(ValueClause clause, ParameterExpression parameter);
    }
}