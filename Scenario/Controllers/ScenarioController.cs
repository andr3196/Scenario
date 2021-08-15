using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scenario.Application;
using Scenario.Services;

namespace Scenario.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScenarioController : Controller
    {
        private readonly IScenarioService scenarioService;

        public ScenarioController(IScenarioService scenarioService)
        {
            this.scenarioService = scenarioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetScenariosList(CancellationToken cancellationToken)
        {
            var scenarios = await scenarioService.GetScenariosList(cancellationToken);
            return Ok(scenarios);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ScenarioCreateDto create, CancellationToken cancellationToken)
        {
            var createResult = await scenarioService.Create(create, cancellationToken);
            return Ok(createResult);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> UpdateRule(long id, CancellationToken cancellationToken)
        {
            await scenarioService.Delete(id, cancellationToken);
            return Ok();
        }
    }
}
