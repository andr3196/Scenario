using System;
using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Configuration;
using Scenario.Domain.Modeling.Models.Constants;
using Scenario.Domain.Modeling.Models.Filters;
using Scenario.Domain.Modeling.Models.Logicals;
using Scenario.Domain.Modeling.Services;

namespace Scenario.Domain.Modeling
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScenarioModelling(this IServiceCollection services, ScenarioConfiguration configuration)
        {
            services.AddSingleton<AssemblyProvider>(_ => () => configuration.DomainAssemblies);
            services.Scan(scanner => scanner
                .FromAssemblyOf<Properties.AssemblyReference>()
                .AddClasses(classes => classes.AssignableTo<IScenarioService>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
            services.Scan(scanner => scanner
                .FromAssemblyOf<Properties.AssemblyReference>()
                .AddClasses(classes => classes.AssignableTo<ILogical>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
            services.Scan(scanner => scanner
                .FromAssemblyOf<Properties.AssemblyReference>()
                .AddClasses(classes => classes.AssignableTo<IConstant>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
            services.Scan(scanner => scanner
                .FromAssemblyOf<Properties.AssemblyReference>()
                .AddClasses(classes => classes.AssignableTo<IFilter>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
            return services;
        }
    }
}
