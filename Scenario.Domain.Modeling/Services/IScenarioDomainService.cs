using System;
using System.Reflection;
using Scenario.Domain.Modeling.Models;

namespace Scenario.Domain.Modeling.Services
{
    public interface IScenarioDomainService : IScenarioService
    {

        ScenarioSetup GetScenarioSetup();
    }
}
