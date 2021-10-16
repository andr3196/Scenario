using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Api;
using Scenario.Application;
using Scenario.Domain;
using Scenario.Domain.Modeling;

namespace Scenario
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScenario(this IServiceCollection services, Assembly domainAssembly)
        {
            return services
                .AddScenarioDomain(domainAssembly)
                .AddScenarioModelling(domainAssembly)
                .AddScenarioApplication()
                .AddScenarioApi();
        }
    }
}
