using System;
using System.Linq.Expressions;
using Scenario.Application;
using Scenario.Domain.ScenarioDefinitions;

namespace Scenario.Services
{
    public interface IScenarioConsequenceExpressionBuilder
    {
        Expression<ScenarioEventHandlerAsync> BuildExpression(ScenarioConsequence consequenceDto, Domain.Modeling.Models.ScenarioSetup model);
    }
}
