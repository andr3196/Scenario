using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Scenario.Domain.Clauses;
using Scenario.Domain.Modeling.Models.Constants;
using Scenario.Domain.SharedTypes;
using Scenario.ErrorHandling;

namespace Scenario.Services.ExpressionBuilding
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

        public Expression GetExpression(ValueWhereClause clause, ParameterExpression parameter)
        {
            switch (clause.Type)
            {
                case "Value":
                    var type = domainTypeResolver.ResolveTypeFromKey(clause.ValueType) ?? throw new ScenarioExpressionBuilderException();
                    return Expression.Constant(Convert.ChangeType(clause.Value, type));
                case "Property":
                    return clause.Value.Split('.').Aggregate(parameter as Expression, (aggregatedExpression, propertyName) => Expression.PropertyOrField(aggregatedExpression, propertyName));
                case "Constant":
                    var constant = constants.Where(c => c.Key == clause.Value).First();
                    Expression<Func<object>> func = () => constant.Evaluate();
                    return Expression.Invoke(func);
                default:
                    throw new ArgumentOutOfRangeException($"No expression convertion for value of type {clause.Type}");
            }
        }
    }
}
