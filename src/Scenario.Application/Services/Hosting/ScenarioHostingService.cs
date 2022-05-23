using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Events;

namespace Scenario.Application.Services.Hosting
{
    public class ScenarioHostingService : IScenarioHostingService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IEventSynchronisationService eventSynchronisationService;

        public ScenarioHostingService(IServiceProvider serviceProvider, IEventSynchronisationService eventSynchronisationService)
        {
            this.serviceProvider = serviceProvider;
            this.eventSynchronisationService = eventSynchronisationService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Started event service");
            using var scope = serviceProvider.CreateAsyncScope();
            var scenarioEventService = scope.ServiceProvider.GetRequiredService<IScenarioEventService>();
            return scenarioEventService.LoadAllAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // await ongoing tasks
            return eventSynchronisationService.WaitForTasksToCompleteAsync(cancellationToken);
        }
    }
}
