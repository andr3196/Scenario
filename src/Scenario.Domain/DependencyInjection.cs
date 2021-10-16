using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Domain.Services.EventHandling;
using Scenario.Domain.Services.ExpressionBuilding;
using Scenario.Domain.Services.ScenarioManagement;

namespace Scenario.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScenarioDomain(this IServiceCollection services, Assembly domainAssembly)
        {
            return services
                .AddTransient<IScenarioEventService, ScenarioEventService>()
                .AddExpressionBuilding(domainAssembly)
                .AddScenarioManagement();
        }
    }
}
