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
using Scenario.Domain.SharedTypes;
using Scenario.Services.ExpressionBuilding;

namespace Scenario.Services
{
    public class ScenarioConsequenceExpressionBuilder : IScenarioConsequenceExpressionBuilder
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IValueClauseExpressionBuilder valueClauseExpressionBuilder;
        private readonly IDomainTypeResolver domainTypeResolver;

        public ScenarioConsequenceExpressionBuilder(
            IServiceProvider serviceProvider,
            IValueClauseExpressionBuilder valueClauseExpressionBuilder,
            IDomainTypeResolver domainTypeResolver)
        {
            this.serviceProvider = serviceProvider;
            this.valueClauseExpressionBuilder = valueClauseExpressionBuilder;
            this.domainTypeResolver = domainTypeResolver;
        }

        public Expression<Func<TInput, CancellationToken, Task>> BuildExpression<TInput>(ScenarioConsequence consequenceDto, Domain.Modeling.Models.ScenarioSetup model)
        {
            var consequenceModel = model.Consequences.SingleOrDefault(c => c.Value == consequenceDto.Key)
                ?? throw new ArgumentException("Consequence key not recognized.");

            var parametersType = domainTypeResolver.ResolveTypeFromKey(consequenceDto.ParametersType)
                ?? throw new ArgumentException("Parameter type not recognized.");

            var handlerType = domainTypeResolver.ResolveTypeFromKey(consequenceModel.HandlerType)
                ?? throw new ArgumentException("HandlerType type not recognized.");
            var handler = serviceProvider.GetRequiredService(handlerType);

            var commandType = domainTypeResolver.ResolveTypeFromKey(consequenceModel.CommandType)
                ?? throw new ArgumentException("command type not recognized.");

            var payloadParameter = Expression.Parameter(typeof(TInput));
            var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken));

            var commandGenerator = GetCommandGeneratorExpression<TInput>(payloadParameter, commandType, consequenceDto.Parameters);
            var handleMethod = handlerType.GetMethod("HandleAsync");

            var command = Expression.Parameter(commandType, "command");
            var result = Expression.Parameter(typeof(Task), "result");

            var body = Expression.Block(
                new[] { command, result },
                Expression.Assign(command, Expression.Convert(Expression.Invoke(commandGenerator, new[] { payloadParameter }), commandType)),
                Expression.Assign(result, Expression.Convert(Expression.Call(Expression.Constant(handler), handleMethod, new[] { command, cancellationTokenParameter }), typeof(Task)))
                );

            return Expression.Lambda<Func<TInput, CancellationToken, Task>>(body, new ParameterExpression[] { payloadParameter, cancellationTokenParameter });
        }

        protected Expression<Func<TInput, object>> GetCommandGeneratorExpression<TInput>(ParameterExpression parameter, Type commandType, Dictionary<string, ValueWhereClause> Parameters)
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
                (commandMember, map) => Expression.Bind(commandMember, map.Value));

            var memberInitExpression = Expression.MemberInit(
                    newCommand,
                    memberBindings);

            return Expression.Lambda<Func<TInput, object>>(memberInitExpression, parameter);
        }
    }
}
