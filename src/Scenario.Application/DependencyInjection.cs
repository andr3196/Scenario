using Microsoft.Extensions.DependencyInjection;
using Scenario.Application.Services;
using Scenario.Application.Services.Hosting;
using Scenario.Application.Services.ScenarioParsing;

namespace Scenario.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScenarioApplication(this IServiceCollection services)
        {
            return services
                .AddTransient<IScenarioService>()
                .AddTransient<IScenarioParsingService>()
                .AddHostedService<IScenarioHostingService>();
        }
    }
}
