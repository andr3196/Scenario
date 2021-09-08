using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Application;

namespace Scenario.Services
{
    public interface IScenarioConsequenceExpressionBuilder
    {
        Expression<Func<TInput, CancellationToken, Task>> BuildExpression<TInput>(ScenarioConsequence consequenceDto, Domain.Modeling.Models.ScenarioSetup model);
    }
}
