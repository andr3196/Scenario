using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Scenario.Domain.Modeling.Services;

namespace Scenario.Api.Controllers
{
    [Route("[controller]")]
    public class ScenarioModelController : Controller
    {
        private readonly IScenarioDomainService scenarioModelService;

        public ScenarioModelController(IScenarioDomainService scenarioModelService)
        {
            this.scenarioModelService = scenarioModelService;
        }

        [Route("")]
        [HttpGet]
        public IActionResult GetModel()
        {
            return Ok(scenarioModelService.GetModel());
        }
    }
}
