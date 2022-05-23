using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Scenario.Domain.Modeling.Models;
using Scenario.Domain.Models.Clauses;
using Scenario.Domain.Services.ExpressionBuilding;
using Scenario.Domain.Shared.TypeHandling;
using Scenario.Test.Services.ExpressionBuilding.Mocks;
using Xunit;

namespace Scenario.Test.Services.ExpressionBuilding
{
    public class ConsequenceBuildingTests
    {
        [Fact]
        public async Task ShouldBuildConsequence()
        {
            // Arrange

            // Values
            // The data provided by the event 
            var personMock = new PersonMock
            {
                Birthday = default(DateTime).AddDays(7),
            };
            var commandMock = new CommandMock();
            var commandProperties = new Dictionary<string, ValueClause>();
            // And the type hereof
            var parameter = Expression.Parameter(typeof(PersonMock));

            // Service mocks
            var resolverMock = new Mock<IDomainTypeResolver>();
            resolverMock.Setup(r => r.ResolveType("CommandMock")).Returns(typeof(CommandMock));
            resolverMock.Setup(r => r.ResolveType("HandlerMock")).Returns(typeof(CommandHandlerMock));

            var commandBuilderMock = new Mock<ICommandExpressionBuilder>();
            Expression<Func<PersonMock, object>> commandExpression = person => commandMock;
            commandBuilderMock
                .Setup(b => b.GetCommandGeneratorExpression<PersonMock>(It.IsAny<ParameterExpression>(), typeof(CommandMock), commandProperties))
                .Returns(commandExpression);
            var handlerMock = new Mock<CommandHandlerMock>();
            handlerMock.Setup(h => h.HandleAsync(commandMock, default)).Returns(Task.CompletedTask);


            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(handlerMock.Object);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            var builder = new ConsequenceExpressionBuilder(serviceProvider, commandBuilderMock.Object, resolverMock.Object);

            var consequenceClause = new ConsequenceClause("HandlerMock", commandProperties);
            var model = new ScenarioDomainModel
            {
                Consequences = new List<Consequence>
                {
                    new Consequence
                    {
                        CommandType = "CommandMock",
                        HandlerType = "HandlerMock",
                        Value = "HandlerMock",
                    },
                }
            };

            // Act
            var lambdaExpression = builder.BuildExpression<PersonMock>(consequenceClause, model);


            Assert.NotNull(lambdaExpression);
            var lambdeFunc = lambdaExpression.Compile();
            var handlerTask = lambdeFunc( personMock, default );
            await handlerTask;
            commandBuilderMock.Verify(b => b.GetCommandGeneratorExpression<PersonMock>(It.IsAny<ParameterExpression>(), typeof(CommandMock), commandProperties), Times.Once);
            handlerMock.Verify(h => h.HandleAsync(commandMock, default), Times.Once);
        }
    }
}
