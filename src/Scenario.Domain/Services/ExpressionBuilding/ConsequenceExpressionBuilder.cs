using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Domain.Clauses;
using Scenario.Domain.Shared.TypeHandling;

namespace Scenario.Services.ExpressionBuilding
{
    public class ConsequenceExpressionBuilder : IConsequenceExpressionBuilder
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ICommandExpressionBuilder commandExpressionBuilder;
        private readonly IDomainTypeResolver domainTypeResolver;

        public ConsequenceExpressionBuilder(
            IServiceProvider serviceProvider,
            ICommandExpressionBuilder commandExpressionBuilder,
            IDomainTypeResolver domainTypeResolver)
        {
            this.serviceProvider = serviceProvider;
            this.commandExpressionBuilder = commandExpressionBuilder;
            this.domainTypeResolver = domainTypeResolver;
        }

        public Expression<Func<TInput, CancellationToken, Task>> BuildExpression<TInput>(ConsequenceClause clause, Domain.Modeling.Models.ScenarioDomainModel model)
        {
            var consequenceModel = model.Consequences.SingleOrDefault(c => c.Value == clause.Key)
                ?? throw new ArgumentException("Consequence key not recognized.");

            var handlerType = domainTypeResolver.ResolveType(consequenceModel.HandlerType)
                ?? throw new ArgumentException("HandlerType type not recognized.");
            var handler = serviceProvider.GetRequiredService(handlerType);

            var commandType = domainTypeResolver.ResolveType(consequenceModel.CommandType)
                ?? throw new ArgumentException("command type not recognized.");

            var payloadParameter = Expression.Parameter(typeof(TInput));
            var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken));

            var commandGenerator = commandExpressionBuilder.GetCommandGeneratorExpression<TInput>(payloadParameter, commandType, clause.Parameters);
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
    }
}
