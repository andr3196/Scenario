using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Scenario.Serialization;

namespace Scenario.Domain.Clauses
{
    [JsonConverter(typeof(RootWhereClauseConverter))]
    public class RootNodeWhereClause
    {
        //[JsonConverter(typeof(PredicateWhereClauseConverter))]
        private IPredicateWhereClause Value { get; set; }

        [JsonConstructor]
        public RootNodeWhereClause(IPredicateWhereClause value)
        {
            Value = value;
        }

        public Expression<Func<object, bool>> GetPredicateExpression(Type entityType)
        {
            var entityParameter = Expression.Parameter(entityType);
            var predicateExpression = Value.GetPredicateExpression(entityParameter);
            return Expression.Lambda<Func<object, bool>>(predicateExpression, entityParameter);
        }
    }
}
