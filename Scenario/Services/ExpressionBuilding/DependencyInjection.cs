using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Domain.SharedTypes;

namespace Scenario.Services.ExpressionBuilding
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddExpressionBuilding(this IServiceCollection services, Assembly domainAssembly)
        {
            return services
                .AddSingleton<IDomainTypeResolver>(provider =>
                {
                    var resolver = new DomainTypeResolver();
                    resolver.RegisterAllTypesFromAssembly(domainAssembly);
                    return resolver;
                })
                .AddTransient<IScenarioConditionExpressionBuilder, RootNodeExpressionBuilder>()
                .AddTransient<IPredicateClauseExpressionBuilder, PredicateClauseExpressionBuilder>()
                .AddTransient<IValueClauseExpressionBuilder, ValueClauseExpressionBuilder>();
        }
    }
}
