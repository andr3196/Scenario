using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Application;
using Scenario.Domain.Clauses;
using Scenario.Domain.ScenarioDefinitions;
namespace Scenario.Services
{
    public class ScenarioConsequenceExpressionBuilder : IScenarioConsequenceExpressionBuilder
    {
        private readonly IServiceProvider serviceProvider;

        public ScenarioConsequenceExpressionBuilder(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Expression<ScenarioEventHandlerAsync> BuildExpression(ScenarioConsequence consequenceDto, Domain.Modeling.Models.ScenarioSetup model)
        {
            var consequenceModel = model.Consequences.SingleOrDefault(c => c.Value == consequenceDto.Key);

            var parametersType = Type.GetType(consequenceDto.ParametersType, throwOnError: true);

            var payloadParameter = Expression.Parameter(parametersType);
            var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken));

            var handlerType = Type.GetType(consequenceModel.HandlerType, throwOnError: true);
            var handler = serviceProvider.GetRequiredService(handlerType);

            var commandType = Type.GetType(consequenceModel.ParameterType, throwOnError: true);

            var commandGenerator = GetCommandGeneratorExpression(payloadParameter, commandType, consequenceDto.Parameters);
            Expression<Func<object, CancellationToken, Task>> handlerInvocation = (input, token) => (Task)handlerType.InvokeMember("HandleAsync", System.Reflection.BindingFlags.Default, null, handler, new object[] { input, token });

            var command = Expression.Parameter(typeof(int), "command");
            var result = Expression.Parameter(typeof(int), "result");

            var body = Expression.Block(
                new[] { command },
                Expression.Assign(command, Expression.Invoke(commandGenerator, new[] { payloadParameter })),
                Expression.Assign(result, Expression.Invoke(handlerInvocation, new[] { command, cancellationTokenParameter }))
                );

            return Expression.Lambda<ScenarioEventHandlerAsync>(body, new ParameterExpression[] { payloadParameter, cancellationTokenParameter });
        }

        protected Expression<Func<object, object>> GetCommandGeneratorExpression(ParameterExpression parameter, Type commandType, Dictionary<string, ValueWhereClause> Parameters)
        {

            var newCommand = Expression.New(commandType);

            var commandMembers = commandType.GetMembers();

            var valueExpressionsMap = Parameters
                .Select(keyVal => new
                {
                    MemberName = keyVal.Key,
                    Expression = keyVal.Value.GetExpression(parameter)
                })
                .ToDictionary(
                    x => x.MemberName,
                    x => x.Expression);

            var memberBindings = commandMembers.Join(
                valueExpressionsMap,
                commandMember => commandMember.Name,
                map => map.Key,
                (commandMember, map) => Expression.Bind(commandMember, map.Value));

            var memberInitExpression = Expression.MemberInit(
                    newCommand,
                    memberBindings);

            return Expression.Lambda<Func<object, object>>(memberInitExpression, parameter);
        }
    }
}
