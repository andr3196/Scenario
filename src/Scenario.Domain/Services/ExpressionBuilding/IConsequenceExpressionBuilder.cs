using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.Models.Clauses;

namespace Scenario.Domain.Services.ExpressionBuilding
{
    public interface IConsequenceExpressionBuilder
    {
        Expression<Func<TInput, CancellationToken, Task>> BuildExpression<TInput>(ConsequenceClause clause, Domain.Modeling.Models.ScenarioDomainModel model);
    }
}
