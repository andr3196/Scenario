using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Api;
using Scenario.Application;
using Scenario.Configuration;
using Scenario.Domain;
using Scenario.Domain.Modeling;
using Scenario.Events;

namespace Scenario
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScenario(this IServiceCollection services, Action<IScenarioConfigurationBuilder> configureScenario)
        {
            var builder = new ScenarioBuilder();
            configureScenario(builder);
            var configuration = builder.Build();
            configuration.RepositoryInstaller(services);
            return services
                .AddScenarioDomain()
                .AddScenarioModelling(configuration)
                .AddScenarioApplication()
                .AddScenarioApi()
                .AddScenarioEventHandling();
        }

        public static IApplicationBuilder UseScenario(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var setupActions = scope.ServiceProvider.GetServices<ScenarioConfigurationAction>();
            foreach (var action in setupActions)
            {
                action(app);
            }
            return app;
        }
    }
}
