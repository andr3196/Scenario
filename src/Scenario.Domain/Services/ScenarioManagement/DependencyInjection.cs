using Microsoft.Extensions.DependencyInjection;

namespace Scenario.Domain.Services.ScenarioManagement
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScenarioManagement(this IServiceCollection services)
        {
            return services
                .AddTransient<IScenarioCreator, ScenarioCreator>();
        }
    }
}
