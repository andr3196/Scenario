using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Scenario.Domain.Models.Clauses;

namespace Scenario.Domain.Services.ExpressionBuilding
{
    public class CommandExpressionBuilder : ICommandExpressionBuilder
    {
        private readonly IValueClauseExpressionBuilder valueClauseExpressionBuilder;

        public CommandExpressionBuilder(IValueClauseExpressionBuilder valueClauseExpressionBuilder)
        {
            this.valueClauseExpressionBuilder = valueClauseExpressionBuilder;
        }

        public Expression<Func<TInput, object>> GetCommandGeneratorExpression<TInput>(ParameterExpression parameter, Type commandType, Dictionary<string, ValueClause> Parameters)
        {

            var newCommand = Expression.New(commandType);

            var commandMembers = commandType.GetProperties();

            var valueExpressionsMap = Parameters
                .Select(keyVal => new
                {
                    MemberName = keyVal.Key,
                    Expression = valueClauseExpressionBuilder.GetExpression(keyVal.Value, parameter)
                })
                .ToDictionary(
                    x => x.MemberName,
                    x => x.Expression);

            var memberBindings = commandMembers.Join(
                valueExpressionsMap,
                commandMember => commandMember.Name,
                map => map.Key,
                (commandMember, map) =>
                {
                    var value = Expression.Convert(Expression.Invoke(map.Value, parameter), commandMember.PropertyType);
                    return Expression.Bind(commandMember, value);
                });

            var memberInitExpression = Expression.MemberInit(
                    newCommand,
                    memberBindings);

            return Expression.Lambda<Func<TInput, object>>(memberInitExpression, parameter);
        }
    }
}
