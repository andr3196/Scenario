using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Domain.Shared.TypeHandling;
using Scenario.Domain.TypeHandling;
using Scenario.Services.ExpressionBuilding;

namespace Scenario.Domain.Services.ExpressionBuilding
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddExpressionBuilding(this IServiceCollection services, Assembly domainAssembly)
        {
            return services
                .AddSingleton<ITypeKeyGenerator, TypeKeyGenerator>()
                .AddSingleton<IDomainTypeResolver>(provider =>
                {
                    var keyGenerator = provider.GetRequiredService<ITypeKeyGenerator>();
                    var resolver = new DomainTypeResolver(keyGenerator);
                    resolver.RegisterAllTypesFromAssembly(domainAssembly);
                    return resolver;
                })
                .AddTransient<ICommandExpressionBuilder, CommandExpressionBuilder>()
                .AddTransient<IConsequenceExpressionBuilder, ConsequenceExpressionBuilder>()
                .AddTransient<IPredicateClauseExpressionBuilder, PredicateClauseExpressionBuilder>()
                .AddTransient<IValueClauseExpressionBuilder, ValueClauseExpressionBuilder>();
        }
    }
}
