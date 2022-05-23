using System.Linq.Expressions;
using Scenario.Domain.Models.Clauses;

namespace Scenario.Domain.Services.ExpressionBuilding
{
    public interface IValueClauseExpressionBuilder
    {
        LambdaExpression GetExpression(ValueClause clause, ParameterExpression parameter);
    }
}