using System;
using Scenario.Contracts;
using Scenario.Domain.Models;

namespace Scenario.Application.Services.ScenarioParsing
{
    public interface IScenarioParsingService
    {
        public bool TryParse(ScenarioDefinitionDto scenario, out ScenarioFlow? scenarioDefinition);

        public bool TryParse(ScenarioDefinitionDto scenario, out ScenarioFlow? scenarioDefinition, out Exception? exception);

        public ScenarioFlow Parse(ScenarioDefinitionDto scenario);
    }
}
