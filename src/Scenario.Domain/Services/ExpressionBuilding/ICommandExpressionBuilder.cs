using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Scenario.Domain.Clauses;

namespace Scenario.Services.ExpressionBuilding
{
    public interface ICommandExpressionBuilder
    {
        Expression<Func<TInput, object>> GetCommandGeneratorExpression<TInput>(ParameterExpression parameter, Type commandType, Dictionary<string, ValueClause> Parameters);
    }
}
