using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scenario.Contracts.Commands;
using Scenario.Contracts.Queries;

namespace Scenario.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScenarioController : Controller
    {
        private readonly IMediator mediator;

        public ScenarioController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetScenariosList(CancellationToken cancellationToken)
        {
            var scenarios = await mediator.Send(new GetAllScenariosQuery(), cancellationToken);
            return Ok(scenarios);
        }
        
        [HttpGet]
        [Route("metadata")]
        public async Task<IActionResult> GetScenarioMetadataList(CancellationToken cancellationToken)
        {
            var scenarios = await mediator.Send(new GetAllScenariosQuery(), cancellationToken);
            return Ok(scenarios);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] CreateScenarioCommand command, CancellationToken cancellationToken)
        {
            var createResult = await mediator.Send(command, cancellationToken);
            return Ok(createResult);
        }

        [HttpPatch]
        [Route("")]
        public async Task<IActionResult> Update([FromBody] UpdateScenarioCommand command, CancellationToken cancellationToken)
        {
            var updateVersionId = await mediator.Send(command, cancellationToken);
            return Ok(updateVersionId);
        }

        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> Delete([FromBody] DeleteScenarioCommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}
