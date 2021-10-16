using Microsoft.Extensions.DependencyInjection;
using Scenario.Api.Controllers;

namespace Scenario.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScenarioApi(this IServiceCollection services)
        {
            return services
                .AddTransient<ScenarioController>()
                .AddTransient<ScenarioModelController>();
        }
    }
}
