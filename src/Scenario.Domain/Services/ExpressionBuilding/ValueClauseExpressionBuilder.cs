using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Scenario.Domain.Exceptions;
using Scenario.Domain.Modeling.Models.Constants;
using Scenario.Domain.Models.Clauses;
using Scenario.Domain.Shared.Contracts;
using Scenario.Domain.Shared.TypeHandling;

namespace Scenario.Domain.Services.ExpressionBuilding
{
    public class ValueClauseExpressionBuilder : IValueClauseExpressionBuilder
    {
        private readonly IDomainTypeResolver domainTypeResolver;
        private readonly IEnumerable<IConstant> constants;

        public ValueClauseExpressionBuilder(IDomainTypeResolver domainTypeResolver, IEnumerable<IConstant> constants)
        {
            this.domainTypeResolver = domainTypeResolver;
            this.constants = constants;
        }

        public LambdaExpression GetExpression(ValueClause clause, ParameterExpression parameter)
        {
            Expression expression;
            switch (clause.Type)
            {
                case ValueContracts.Value:
                    var type = domainTypeResolver.ResolveType(clause.ValueType)
                        ?? throw new ExpressionBuildingException($"Failed to resolve type for key: {clause.ValueType ?? "'null'"}");
                    expression = Expression.Constant(type == typeof(string) ? clause.Value : Convert.ChangeType(clause.Value, type));
                    break;
                case ValueContracts.Property:
                    expression = clause.Value.Split('.').Aggregate((Expression)parameter, (aggregatedExpression, propertyName) => Expression.PropertyOrField(aggregatedExpression, propertyName));
                    break;
                case ValueContracts.Constant:
                    var constant = constants.Where(c => c.Key == clause.Value).FirstOrDefault()
                        ?? throw new ExpressionBuildingException($"Failed to resolve constant for key: {clause.Value ?? "'null'"}");

                    Expression<Func<object>> func = () => constant.Evaluate();
                    expression = Expression.Invoke(func);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"No expression convertion for value of type {clause.Type}");
            }
            return Expression.Lambda(expression, parameter);
        }
    }
}
