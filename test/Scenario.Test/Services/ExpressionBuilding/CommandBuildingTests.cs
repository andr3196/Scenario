using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using Scenario.Domain.Models.Clauses;
using Scenario.Domain.Services.ExpressionBuilding;
using Scenario.Domain.Shared.Contracts;
using Scenario.Domain.Shared.TypeHandling;
using Scenario.Test.Services.ExpressionBuilding.Mocks;
using Xunit;

namespace Scenario.Test.Services.ExpressionBuilding
{
    public class CommandBuildingTests
    {
        [Fact]
        public void ShouldBuildCommandForWithSimpleProperties()
        {
            // Arrange

            // Values
            // The data provided by the event 
            var mockCommand = new PersonMock
            {
                Birthday = default(DateTime).AddDays(7),
            };
            // And the type hereof
            var parameter = Expression.Parameter(typeof(PersonMock));

            // Service mocks
            var resolverMock = new Mock<IDomainTypeResolver>();
            var valueBuilderMock = new Mock<IValueClauseExpressionBuilder>();

            // Define properties
            var stringValueClause = SetupPropertyMock("Test string", valueBuilderMock, resolverMock, parameter);
            var dateTimeValueClause = SetupPropertyMock(default(DateTime).AddDays(2), valueBuilderMock, resolverMock, parameter);
            var intValueClause = SetupPropertyMock(123, valueBuilderMock, resolverMock, parameter);
            var longValueClause = SetupPropertyMock(14534L, valueBuilderMock, resolverMock, parameter);
            var guid = Guid.NewGuid();
            var guidValueClause = SetupPropertyMock(guid, valueBuilderMock, resolverMock, parameter);
            var boolValueClause = SetupPropertyMock(true, valueBuilderMock, resolverMock, parameter);
            var decimalValueClause = SetupPropertyMock(1.5, valueBuilderMock, resolverMock, parameter);
            var nullValueClause = SetupPropertyMock<string?>(null, valueBuilderMock, resolverMock, parameter);


            var parameters = new Dictionary<string, ValueClause>
            {
                { nameof(CommandMock.AStringProperty), stringValueClause },
                { nameof(CommandMock.AnIntProperty), intValueClause },
                { nameof(CommandMock.ABoolProperty), boolValueClause },
                { nameof(CommandMock.ADateTimeProperty), dateTimeValueClause },
                { nameof(CommandMock.ADoubleProperty), decimalValueClause },
                { nameof(CommandMock.AGuidProperty), guidValueClause },
                { nameof(CommandMock.ALongProperty), longValueClause },
                { nameof(CommandMock.ANullProperty), nullValueClause },
            };
            
            var builder = new CommandExpressionBuilder(valueBuilderMock.Object);

            // Act
            var lambdaExpression = builder.GetCommandGeneratorExpression<PersonMock>(parameter, typeof(CommandMock), parameters);


            Assert.NotNull(lambdaExpression);
            var lambdeFunc = lambdaExpression.Compile();
            var command = lambdeFunc( mockCommand )  as CommandMock;
            Assert.NotNull(command);
            Assert.Equal("Test string", command!.AStringProperty);
            Assert.Equal(123, command!.AnIntProperty);
            Assert.True(command!.ABoolProperty);
            Assert.Equal(default(DateTime).AddDays(2), command!.ADateTimeProperty);
            Assert.Equal(1.5, command!.ADoubleProperty);
            Assert.Equal(guid, command!.AGuidProperty);
            Assert.Equal(14534L, command!.ALongProperty);
            Assert.Null(command!.ANullProperty);
        }

        private ValueClause SetupPropertyMock<TType>(TType propertyValue, Mock<IValueClauseExpressionBuilder> valueBuilderMock, Mock<IDomainTypeResolver> resolverMock, ParameterExpression parameter)
        {
            var propertyType = typeof(TType);
            resolverMock.Setup(resolver => resolver.ResolveType(propertyType.Name)).Returns(propertyType);
            var valueClause = new ValueClause(string.Empty, propertyType.Name, string.Empty);
            Expression<Func<PersonMock, object?>> stringPropertyExpression = person => propertyValue;
            valueBuilderMock.Setup(b => b.GetExpression(valueClause, parameter)).Returns(stringPropertyExpression);
            return valueClause;
        }
    }
}
