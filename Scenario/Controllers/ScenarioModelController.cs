using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scenario.Domain.Modeling.Models;
using Scenario.Services;

namespace Scenario.Controllers
{
    [Route("[controller]")]
    public class ScenarioModelController : Controller
    {
        private readonly IScenarioModelService scenarioModelService;

        public ScenarioModelController(IScenarioModelService scenarioModelService)
        {
            this.scenarioModelService = scenarioModelService;
        }

        [Route("")]
        [HttpGet]
        public IActionResult GetModel(CancellationToken cancellationToken)
        {
            return Ok(scenarioModelService.GetModel());
        }
    }
}
