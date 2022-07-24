using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Scenario.Contracts.Commands;

namespace Scenario.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScenarioDefinitionController : ControllerBase
    {
        private readonly IMediator mediator;

        public ScenarioDefinitionController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        
        [HttpPost]
        [Route("{definition}")]
        public async Task<IActionResult> Create([FromRoute] string definition, CancellationToken cancellationToken)
        {
            var createResult = await mediator.Send(new CreateScenarioDefinition(definition), cancellationToken);
            return Ok(createResult);
        }
        
    }   
}