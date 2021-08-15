using System;
using System.Linq.Expressions;

namespace Scenario.Domain.Clauses
{
    public interface IPredicateWhereClause
    {
        Expression<Func<object, bool>> GetPredicateExpression(ParameterExpression parameter);

        public string Discriminator { get; }
    }
}
