using Scenario.Domain.Modeling.Models;

namespace Scenario.Services
{
    public interface IScenarioModelService
    {
        ScenarioSetup GetModel();
    }
}
