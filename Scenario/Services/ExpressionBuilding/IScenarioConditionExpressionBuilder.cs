using System;
using System.Linq.Expressions;
using Scenario.Application;
using Scenario.Domain.Clauses;
using Scenario.Domain.ScenarioDefinitions;

namespace Scenario.Services.ExpressionBuilding
{
    public interface IScenarioConditionExpressionBuilder
    {
        Expression<Func<TInput, bool>> BuildExpression<TInput>(RootNodeWhereClause conditionClause, Type entityType);
    }
}
