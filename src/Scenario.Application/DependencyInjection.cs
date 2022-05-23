using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Application.Properties;
using Scenario.Application.Services.Hosting;
using Scenario.Application.Services.ScenarioParsing;
using Scenario.Domain;

namespace Scenario.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScenarioApplication(this IServiceCollection services)
        {
            return services
                .AddTransient<IScenarioParsingService, ScenarioParsingService>()
                .AddAutoMapper(typeof(AssemblyReference))
                .AddHostedService<ScenarioHostingService>()
                .AddHandlers()
                .AddSerialization();
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            return services.Scan(scrutor => scrutor
                .FromAssemblyOf<AssemblyReference>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
                .AsImplementedInterfaces()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces()
                .AddClasses(classes => classes.AssignableTo(typeof(ITypeConverter<,>)))
                .AsImplementedInterfaces()
                .AddClasses(classes => classes.AssignableTo(typeof(IValueConverter<,>)))
                .AsImplementedInterfaces()
            );
        }
    }
}
