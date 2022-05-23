using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Scenario.Domain.Models.Clauses;

namespace Scenario.Domain.Services.ExpressionBuilding
{
    public interface ICommandExpressionBuilder
    {
        Expression<Func<TInput, object>> GetCommandGeneratorExpression<TInput>(ParameterExpression parameter, Type commandType, Dictionary<string, ValueClause> Parameters);
    }
}
