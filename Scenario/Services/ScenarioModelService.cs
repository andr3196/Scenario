using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.Modeling.Models;
using Scenario.Domain.Modeling.Services;

namespace Scenario.Services
{
    public class ScenarioModelService: IScenarioModelService
    {
        private readonly IScenarioDomainService scenarioDomainService;

        public ScenarioModelService(IScenarioDomainService scenarioDomainService)
        {
            this.scenarioDomainService = scenarioDomainService;
        }

        public ScenarioSetup GetModel()
        {
            return scenarioDomainService.GetScenarioSetup();
        }
    }
}
