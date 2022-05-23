using Microsoft.Extensions.DependencyInjection;

namespace Scenario.Events;

public static class DependencyInjection
{
    public static IServiceCollection AddScenarioEventHandling(this IServiceCollection services)
    {
        return services
            .AddTransient<IScenarioEventService, ScenarioEventService>()
            .AddTransient<IEventSynchronisationService,EventSynchronisationService>();
    }
}