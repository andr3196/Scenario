using System;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Services;

namespace Scenario
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScenario(this IServiceCollection services)
        {
            services.AddSingleton<IScenarioService, ScenarioService>();
            services.AddSingleton<IScenarioModelService, ScenarioModelService>();
            return services;
        }
    }
}
