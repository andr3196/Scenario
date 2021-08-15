using System;
using System.Linq.Expressions;

namespace Scenario.Domain.Clauses
{
    public abstract class WhereClauseNode<TExpression> where TExpression : Expression
    {
        public abstract Expression GetExpression(IServiceProvider serviceProvider);
    }
}
