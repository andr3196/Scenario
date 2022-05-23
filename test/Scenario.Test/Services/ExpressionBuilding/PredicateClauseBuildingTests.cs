using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using Scenario.Domain.Modeling.Models.Filters;
using Scenario.Domain.Modeling.Models.Logicals;
using Scenario.Domain.Models.Clauses;
using Scenario.Domain.Services.ExpressionBuilding;
using Scenario.Domain.Shared.TypeHandling;
using Scenario.Test.Services.ExpressionBuilding.Mocks;
using Scenario.Test.Services.ExpressionBuilding.Mocks.Filters;
using Xunit;

namespace Scenario.Test.Services.ExpressionBuilding
{
    public class PredicateClauseBuildingTests
    {
        [Fact]
        public void ShouldBuildCorrectExpressionWhenUnaryOperator1()
        {
            // Arrange

            /*
             * Return expression for:
             * {
             *    discriminator: 'UnaryOperatorClause',
             *    operator: "is test"
             *    value: {
             *      type: "test1"
             *    }
             */

            // Values
            // The data provided by the event 
            var mockCommand = new PersonMock();
            // And the type hereof
            var parameter = Expression.Parameter(typeof(PersonMock));

            // The received clauses to build the expression from:
            var valueCluaseMock = new ValueClause(string.Empty, string.Empty, string.Empty);
            var unaryClause = new UnaryOperatorClause(valueCluaseMock, "is test");

            // Service mocks

            var resolverMock = new Mock<IDomainTypeResolver>();
            resolverMock.Setup(resolver => resolver.ResolveType(It.IsAny<string>())).Returns(typeof(PersonMock));

            // Create a mock filter returning a fixed value regardless of arguments
            var evaluateMock = new Mock<Func<PersonMock, bool>>();
            evaluateMock.Setup(f => f(mockCommand)).Returns(true);
            var filter = new GenericUnaryFilterMock<PersonMock>(new Dictionary<string, Func<PersonMock, bool>>
            {
                {"is test", evaluateMock.Object },
            });

            // Mock the expression from the value clause
            var valueBuilderMock = new Mock<IValueClauseExpressionBuilder>();
            Expression<Func<PersonMock, object>> valueClauseExpressionMock = person => person;
            valueBuilderMock.Setup(b => b.GetExpression(valueCluaseMock, parameter)).Returns(valueClauseExpressionMock);

            
            var builder = new PredicateClauseExpressionBuilder(
                new List<IFilter> { filter },
                new List<ILogical>(),
                valueBuilderMock.Object,
                resolverMock.Object);

            // Act
            var lambdaExpression = builder.GetPredicateExpression<PersonMock>(unaryClause, parameter);


            Assert.NotNull(lambdaExpression);
            var lambdeFunc = lambdaExpression.Compile();
            var value = lambdeFunc( mockCommand );
            Assert.True(value);
            evaluateMock.Verify(f => f(mockCommand), Times.Once);
            valueBuilderMock.Verify(b => b.GetExpression(valueCluaseMock, parameter), Times.Once);
        }

        [Fact]
        public void ShouldBuildCorrectExpressionWhenUnaryOperator2()
        {
            // Arrange

            /*
             * Return expression for:
             * {
             *    discriminator: 'UnaryOperatorClause',
             *    operator: "is test"
             *    value: {
             *      type: "test1"
             *    }
             */

            // Values
            // The data provided by the event 
            var mockCommand = new PersonMock();
            // And the type hereof
            var parameter = Expression.Parameter(typeof(PersonMock));

            // The received clauses to build the expression from:
            var valueCluaseMock = new ValueClause(string.Empty, string.Empty, string.Empty);
            var unaryClause = new UnaryOperatorClause(valueCluaseMock, "is test");

            // Service mocks

            var resolverMock = new Mock<IDomainTypeResolver>();
            resolverMock.Setup(resolver => resolver.ResolveType(It.IsAny<string>())).Returns(typeof(PersonMock));

            // Create a mock filter returning a fixed value regardless of arguments
            var evaluateMock = new Mock<Func<PersonMock, bool>>();
            evaluateMock.Setup(f => f(mockCommand)).Returns(false);
            var filter = new GenericUnaryFilterMock<PersonMock>(new Dictionary<string, Func<PersonMock, bool>>
            {
                {"is test", evaluateMock.Object },
            });

            // Mock the expression from the value clause
            var valueBuilderMock = new Mock<IValueClauseExpressionBuilder>();
            Expression<Func<PersonMock, object>> valueClauseExpressionMock = person => person;
            valueBuilderMock.Setup(b => b.GetExpression(valueCluaseMock, parameter)).Returns(valueClauseExpressionMock);


            var builder = new PredicateClauseExpressionBuilder(
                new List<IFilter> { filter },
                new List<ILogical>(),
                valueBuilderMock.Object,
                resolverMock.Object);

            // Act
            var lambdaExpression = builder.GetPredicateExpression<PersonMock>(unaryClause, parameter);


            Assert.NotNull(lambdaExpression);
            var lambdeFunc = lambdaExpression.Compile();
            var value = lambdeFunc(mockCommand);
            Assert.False(value);
            evaluateMock.Verify(f => f(mockCommand), Times.Once);
            valueBuilderMock.Verify(b => b.GetExpression(valueCluaseMock, parameter), Times.Once);
        }

        [Fact]
        public void ShouldBuildCorrectExpressionWhenBinaryOperator1()
        {
            // Arrange

            /*
             * Return expression for:
             * {
             *    discriminator: 'BinaryOperatorClause',
             *    operator: "is test"
             *    left: *mocked*,
             *    right: *mocked*,
             */

            // Values
            // The data provided by the event 
            var mockCommand = new PersonMock();
            // And the type hereof
            var parameter = Expression.Parameter(typeof(PersonMock));

            // The received clauses to build the expression from:
            var leftClauseMock = new ValueClause(string.Empty, "stringType", string.Empty);
            var rightClauseMock = new ValueClause(string.Empty, "numberType", string.Empty);
            var binaryClause = new BinaryOperatorClause(leftClauseMock, rightClauseMock, "is test");

            // Service mocks

            var resolverMock = new Mock<IDomainTypeResolver>();
            resolverMock.Setup(resolver => resolver.ResolveType("stringType")).Returns(typeof(string));
            resolverMock.Setup(resolver => resolver.ResolveType("numberType")).Returns(typeof(int));

            // Create a mock filter returning a fixed value regardless of arguments
            var evaluateMock = new Mock<Func<string, int, bool>>();
            evaluateMock.Setup(f => f("test", 123)).Returns(true);
            var filter = new GenericBinaryFilterMock<string, int>(new Dictionary<string, Func<string, int, bool>>
            {
                {"is test", evaluateMock.Object },
            });

            // Mock the expressions from the value clauses
            var valueBuilderMock = new Mock<IValueClauseExpressionBuilder>();
            Expression<Func<PersonMock, object>> leftExpression = person => "test";
            Expression<Func<PersonMock, object>> rightExpression = person => 123;
            valueBuilderMock.Setup(b => b.GetExpression(leftClauseMock, parameter)).Returns(leftExpression);
            valueBuilderMock.Setup(b => b.GetExpression(rightClauseMock, parameter)).Returns(rightExpression);


            var builder = new PredicateClauseExpressionBuilder(
                new List<IFilter> { filter },
                new List<ILogical>(),
                valueBuilderMock.Object,
                resolverMock.Object);

            // Act
            var lambdaExpression = builder.GetPredicateExpression<PersonMock>(binaryClause, parameter);


            Assert.NotNull(lambdaExpression);
            var lambdeFunc = lambdaExpression.Compile();
            var value = lambdeFunc(mockCommand);
            Assert.True(value);
            evaluateMock.Verify(f => f("test", 123), Times.Once);
            valueBuilderMock.Verify(b => b.GetExpression(leftClauseMock, parameter), Times.Once);
            valueBuilderMock.Verify(b => b.GetExpression(rightClauseMock, parameter), Times.Once);
        }

        [Fact]
        public void ShouldBuildCorrectExpressionWhenUnaryPredicate1()
        {
            // Arrange

            /*
             * Return expression for:
             * {
             *    discriminator: 'UnaryPredicateClause',
             *    value: *mocked*
             * }
             */

            // Values
            // The data provided by the event 
            var mockCommand = new PersonMock();
            // And the type hereof
            var parameter = Expression.Parameter(typeof(PersonMock));

            // The received clauses to build the expression from:
            var valueClauseMock = new ValueClause(string.Empty, string.Empty, string.Empty);
            var unaryOperatorClause = new UnaryOperatorClause(valueClauseMock, "mocked");
            var unaryPredicateClause = new UnaryPredicateClause(unaryOperatorClause);

            // Service mocks

            var resolverMock = new Mock<IDomainTypeResolver>();
            resolverMock.Setup(resolver => resolver.ResolveType(string.Empty)).Returns(typeof(string));

            // Create a mock filter returning a fixed value regardless of arguments
            var evaluateMock = new Mock<Func<string, bool>>();
            evaluateMock.Setup(f => f("test")).Returns(true);
            var filter = new GenericUnaryFilterMock<string>(new Dictionary<string, Func<string, bool>>
            {
                {"mocked", evaluateMock.Object },
            });

            // Mock the expressions from the value clauses
            var valueBuilderMock = new Mock<IValueClauseExpressionBuilder>();
            Expression<Func<PersonMock, object>> valueExpression = person => "test";
            valueBuilderMock.Setup(b => b.GetExpression(valueClauseMock, parameter)).Returns(valueExpression);


            var builder = new PredicateClauseExpressionBuilder(
                new List<IFilter> { filter },
                new List<ILogical>(),
                valueBuilderMock.Object,
                resolverMock.Object);

            // Act
            var lambdaExpression = builder.GetPredicateExpression<PersonMock>(unaryPredicateClause, parameter);


            Assert.NotNull(lambdaExpression);
            var lambdeFunc = lambdaExpression.Compile();
            var value = lambdeFunc(mockCommand);
            Assert.True(value);
            evaluateMock.Verify(f => f("test"), Times.Once);
            valueBuilderMock.Verify(b => b.GetExpression(valueClauseMock, parameter), Times.Once);
        }

        [Fact]
        public void ShouldBuildCorrectExpressionWhenBinaryPredicate1()
        {
            // Arrange

            /*
             * Return expression for:
             * {
             *    discriminator: 'UnaryPredicateClause',
             *    value: *mocked*
             * }
             */

            // Values
            // The data provided by the event 
            var mockCommand = new PersonMock();
            // And the type hereof
            var parameter = Expression.Parameter(typeof(PersonMock));

            // Shared random values:
            var aDateTimeValue = default(DateTime).AddDays(17);
            var aLongValue = 38L;
            var aBoolValue = true;

            // The received clauses to build the expression from:
            var valueClauseMock = new ValueClause(string.Empty, "dateTimeType", string.Empty);
            var leftClauseMock = new UnaryOperatorClause(valueClauseMock, "operator1");
            var valueClauseMock1 = new ValueClause(string.Empty, "longType", string.Empty);
            var valueClauseMock2 = new ValueClause(string.Empty, "boolType", string.Empty);
            var rightClauseMock = new BinaryOperatorClause(valueClauseMock1, valueClauseMock2, "operator2");
            var binaryPredicateClause = new BinaryPredicateClause(leftClauseMock, rightClauseMock, "LOGICAL_AND");

            // Service mock
            var resolverMock = new Mock<IDomainTypeResolver>();
            resolverMock.Setup(resolver => resolver.ResolveType("dateTimeType")).Returns(typeof(DateTime));
            resolverMock.Setup(resolver => resolver.ResolveType("longType")).Returns(typeof(long));
            resolverMock.Setup(resolver => resolver.ResolveType("boolType")).Returns(typeof(bool));

            // Mock the expressions from the value clauses (values from constants or from the PersonMock)
            var valueBuilderMock = new Mock<IValueClauseExpressionBuilder>();
            Expression<Func<PersonMock, object>> valueDateTimeExpression = person => aDateTimeValue;
            Expression<Func<PersonMock, object>> valueLongExpression = person => aLongValue;
            Expression<Func<PersonMock, object>> valueBoolExpression = person => aBoolValue;
            valueBuilderMock.Setup(b => b.GetExpression(valueClauseMock, parameter)).Returns(valueDateTimeExpression);
            valueBuilderMock.Setup(b => b.GetExpression(valueClauseMock1, parameter)).Returns(valueLongExpression);
            valueBuilderMock.Setup(b => b.GetExpression(valueClauseMock2, parameter)).Returns(valueBoolExpression);

            // Create a mock filter returning a fixed value
            var dateTimeEvaluator = new Mock<Func<DateTime, bool>>();
            dateTimeEvaluator.Setup(f => f(aDateTimeValue)).Returns(true);

            var longBoolEvaluator = new Mock<Func<long, bool, bool>>();
            longBoolEvaluator.Setup(f => f(aLongValue, aBoolValue)).Returns(true);

            var filter1 = new GenericUnaryFilterMock<DateTime>(new Dictionary<string, Func<DateTime, bool>>
            {
                {"operator1", dateTimeEvaluator.Object },
            });

            var filter2 = new GenericBinaryFilterMock<long, bool>(new Dictionary<string, Func<long, bool, bool>>
            {
                {"operator2", longBoolEvaluator.Object },
            });


            var builder = new PredicateClauseExpressionBuilder(
                new List<IFilter> { filter1, filter2 },
                new List<ILogical> { new LogicalAnd() },
                valueBuilderMock.Object,
                resolverMock.Object);

            // Act
            var lambdaExpression = builder.GetPredicateExpression<PersonMock>(binaryPredicateClause, parameter);


            Assert.NotNull(lambdaExpression);
            var lambdeFunc = lambdaExpression.Compile();
            var value = lambdeFunc(mockCommand);
            Assert.True(value);

            dateTimeEvaluator.Verify(f => f(aDateTimeValue), Times.Once);
            longBoolEvaluator.Verify(f => f(aLongValue, aBoolValue), Times.Once);

            valueBuilderMock.Verify(b => b.GetExpression(valueClauseMock, parameter), Times.Once);
            valueBuilderMock.Verify(b => b.GetExpression(valueClauseMock1, parameter), Times.Once);
            valueBuilderMock.Verify(b => b.GetExpression(valueClauseMock2, parameter), Times.Once);
        }
    }
}
