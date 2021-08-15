using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Scenario.Domain.Modeling.Models.Constants;

namespace Scenario.Domain.Clauses
{
    public class ValueWhereClause
    {
        public IEnumerable<IConstant> constants;

        public ValueWhereClause(IEnumerable<IConstant> constants)
        {
            this.constants = constants;
        }

        public string Type { get; set; }

        public Type ValueType { get; set; }

        public string Value { get; set; }

        public Expression GetExpression(Expression parameter)
        {
            switch (Type)
            {
                case "Value":
                    return Expression.Constant(Convert.ChangeType(Value, ValueType));
                case "Property":
                    return Expression.PropertyOrField(parameter, Value);
                case "Constant":
                    var constant = constants.Where(c => c.Key == Value).First();
                    Expression<Func<object>> func = () => constant.Evaluate();
                    return Expression.Invoke(func);
                default:
                    throw new ArgumentOutOfRangeException($"No expression convertion for value of type {Type}");
            }
        }
    }
}
