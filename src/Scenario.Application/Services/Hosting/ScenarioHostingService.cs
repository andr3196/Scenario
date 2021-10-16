using System;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.Services.EventHandling;

namespace Scenario.Application.Services.Hosting
{
    public class ScenarioHostingService : IScenarioHostingService
    {
        private readonly IScenarioEventService scenarioEventService;

        public ScenarioHostingService(IScenarioEventService scenarioEventService)
        {
            this.scenarioEventService = scenarioEventService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Started event service");
            return scenarioEventService.LoadAllAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // await ongoing tasks
            return scenarioEventService.WaitForTasksToCompleteAsync(cancellationToken);
        }
    }
}
