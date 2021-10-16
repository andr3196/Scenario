using System;
using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Domain.Modeling.Models.Constants;
using Scenario.Domain.Modeling.Models.Filters;
using Scenario.Domain.Modeling.Models.Logicals;
using Scenario.Domain.Modeling.Services;

namespace Scenario.Domain.Modeling
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScenarioModelling(this IServiceCollection services, Assembly domainAssembly)
        {
            services.AddSingleton<AssemblyProvider>(provider => () => domainAssembly);
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
