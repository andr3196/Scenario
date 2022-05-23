using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using Moq;
using Scenario.Domain.Exceptions;
using Scenario.Domain.Modeling.Models;
using Scenario.Domain.Modeling.Models.Constants;
using Scenario.Domain.Models.Clauses;
using Scenario.Domain.Services.ExpressionBuilding;
using Scenario.Domain.Shared.TypeHandling;
using Scenario.Domain.TypeHandling;
using Scenario.Test.Services.ExpressionBuilding.Mocks;
using Scenario.Test.TypeHandling.Mocks;
using Xunit;

namespace Scenario.Test.Services.ExpressionBuilding
{
    public class ValueClauseBuildingTests
    {
        [Theory]
        [MemberData(nameof(GetValueClauseValueScenarios))]
        public void ShouldYieldValueFromValueFromValueClause(ValueClause clause, Type valueType, object expectedValue)
        {
            // Arrange
            var resolverMock = new Mock<IDomainTypeResolver>();
            resolverMock.Setup(resolver => resolver.ResolveType(It.IsAny<string>())).Returns(valueType);

            var parameter = Expression.Parameter(typeof(string));
            var builder = new ValueClauseExpressionBuilder(resolverMock.Object, new List<IConstant>());

            // Act -
            var lambdaExpression = builder.GetExpression(clause, parameter);

            Assert.NotNull(lambdaExpression);
            var lambdeFunc = lambdaExpression.Compile();
            var value = lambdeFunc.DynamicInvoke(new object[] { "Testing" });

            Assert.Equal(expectedValue, value);
        }

        public static IEnumerable<object[]> GetValueClauseValueScenarios()
        {
            return new List<object[]>
            {
                // A value of type string
                new object[] {
                    new ValueClause(
                        type: "Value",
                        valueType: "string",
                        value: "Test string"),
                    typeof(string),
                    "Test string"
                },
                // A value of type int
                new object[] {
                    new ValueClause(
                        type: "Value",
                        valueType: "int",
                        value: "1234"),
                    typeof(int),
                    1234
                },
                // A value of type long
                new object[] {
                    new ValueClause(
                        type: "Value",
                        valueType: "long",
                        value: "12"),
                    typeof(long),
                    12L
                },
                // A value of type bool
                new object[] {
                    new ValueClause(
                        type: "Value",
                        valueType: "bool",
                        value: "true"),
                    typeof(bool),
                    true
                },
                // A value of type DateTime
                new object[] {
                    new ValueClause(
                        type: "Value",
                        valueType: "DateTime",
                        value: default(DateTime).AddDays(3).ToString("o")),
                    typeof(DateTime),
                    default(DateTime).AddDays(3)
                },
            };
        }

        [Fact]
        public void ShouldThrowWhenIncorrectTypeInValueClause()
        {
            // Arrange resolver to resolve type correctly 
            var resolverMock = new Mock<IDomainTypeResolver>();
            resolverMock.Setup(resolver => resolver.ResolveType("int")).Returns(typeof(int));

            var parameter = Expression.Parameter(typeof(string));
            var builder = new ValueClauseExpressionBuilder(resolverMock.Object, new List<IConstant>());

            var clause = new ValueClause
            (
                type: "Value",
                value: "A text value",
                valueType: "abc"
            );

            // Act - Assert
            Assert.Throws<ExpressionBuildingException>(() => builder.GetExpression(clause, parameter));
        }

        [Fact]
        public void ShouldThrowWhenValueCannotBeConverted()
        {
            // Arrange resolver to resolve type correctly 
            var resolverMock = new Mock<IDomainTypeResolver>();
            resolverMock.Setup(resolver => resolver.ResolveType("int")).Returns(typeof(int));

            var parameter = Expression.Parameter(typeof(string));
            var builder = new ValueClauseExpressionBuilder(resolverMock.Object, new List<IConstant>());

            var clause = new ValueClause
            (
                type: "Value",
                value: "A text value",
                valueType: "int"
            );

            // Act - Assert
            Assert.Throws<FormatException>(() => builder.GetExpression(clause, parameter));
        }

        [Theory]
        [MemberData(nameof(GetValueClausePropertyScenarios))]
        public void ShouldYieldPropertyValueFromParameter(ValueClause clause, object parameterValue, Type parameterType, object expectedValue)
        {
            // Arrange
            var resolverMock = new Mock<IDomainTypeResolver>();

            var parameter = Expression.Parameter(parameterType);
            var builder = new ValueClauseExpressionBuilder(resolverMock.Object, new List<IConstant>());

            // Act -
            var lambdaExpression = builder.GetExpression(clause, parameter);

            Assert.NotNull(lambdaExpression);
            var lambdeFunc = lambdaExpression.Compile();
            var value = lambdeFunc.DynamicInvoke(new object[] { parameterValue });

            Assert.Equal(expectedValue, value);
        }

        public static IEnumerable<object[]> GetValueClausePropertyScenarios()
        {
            return new List<object[]>
            {
                // A direct property
                new object[] {
                    new ValueClause(
                        type: "Property",
                        valueType: string.Empty,
                        value: "Name"),
                    new PersonMock
                    {
                        Name = "Test name",
                    },
                    typeof(PersonMock),
                    "Test name"
                },
                // A 1 layer nested property
                new object[] {
                    new ValueClause(
                        type: "Property",
                        valueType: string.Empty,
                        value: "Parent.Age"),
                    new PersonMock
                    {
                        Name = "Test name",
                        Parent = new PersonMock
                        {
                            Age = 40,
                        },
                    },
                    typeof(PersonMock),
                    40
                },
                // A 2 layer nested property
                new object[] {
                    new ValueClause(
                        type: "Property",
                        valueType: string.Empty,
                        value: "Parent.Parent.Birthday"),
                    new PersonMock
                    {
                        Parent = new PersonMock
                        {
                            Parent = new PersonMock
                            {
                                Birthday = default(DateTime).AddDays(4),
                            },
                        },
                    },
                    typeof(PersonMock),
                    default(DateTime).AddDays(4)
                },
            };
        }



        [Theory]
        [MemberData(nameof(GetValueClauseConstantScenarios))]
        public void ShouldYieldConstantValue(ValueClause clause, object expectedValue)
        {
            // Arrange
            var resolverMock = new Mock<IDomainTypeResolver>();

            var parameter = Expression.Parameter(typeof(string));
            var builder = new ValueClauseExpressionBuilder(resolverMock.Object, new List<IConstant>
            {
                new CurrentTimeConstantMock(),
                new CurrentUserIdMock(),
            });

            // Act -
            var lambdaExpression = builder.GetExpression(clause, parameter);

            Assert.NotNull(lambdaExpression);
            var lambdeFunc = lambdaExpression.Compile();
            var value = lambdeFunc.DynamicInvoke(new object[] { "Unused value" });

            Assert.Equal(expectedValue, value);
        }

        public static IEnumerable<object[]> GetValueClauseConstantScenarios()
        {
            return new List<object[]>
            {
                new object[] {
                    new ValueClause(
                        type: "Constant",
                        valueType: string.Empty,
                        value: "CURRENT_TIME"),
                    new CurrentTimeConstantMock().Value,
                },
                // A 1 layer nested property
                new object[] {
                    new ValueClause(
                        type: "Constant",
                        valueType: string.Empty,
                        value: "CURRENT_USERID"),
                    new CurrentUserIdMock().Value
                }
            };
        }
    }
}
