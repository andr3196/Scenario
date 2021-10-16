using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scenario.Application.Models;
using Scenario.Application.Services;

namespace Scenario.Api.Controllers
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

        [HttpPatch]
        public async Task<IActionResult> Update(ScenarioUpdateDto update, CancellationToken cancellationToken)
        {
            var createResult = await scenarioService.Update(update, cancellationToken);
            return Ok(createResult);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
        {
            await scenarioService.Delete(id, cancellationToken);
            return Ok();
        }
    }
}
