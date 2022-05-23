using Microsoft.Extensions.DependencyInjection;
using Scenario.Domain.Serialization.JsonConvertion;
using Scenario.Domain.Services.ExpressionBuilding;
using Scenario.Domain.Services.ScenarioManagement;
using Scenario.Domain.Shared.TypeHandling;
using Scenario.Domain.TypeHandling;

namespace Scenario.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScenarioDomain(this IServiceCollection services)
        {
            return services
                .AddTransient<IDomainTypeResolver, DomainTypeResolver>()
                .AddTransient<ITypeKeyGenerator, TypeKeyGenerator>()
                .AddTransient<IConsequenceExpressionBuilder, ConsequenceExpressionBuilder>()
                .AddTransient<ICommandExpressionBuilder, CommandExpressionBuilder>()
                .AddTransient<IConditionExpressionBuilder,ConditionExpressionBuilder>()
                .AddTransient<IValueClauseExpressionBuilder, ValueClauseExpressionBuilder>()
                .AddTransient<IPredicateClauseExpressionBuilder, PredicateClauseExpressionBuilder>()
                .AddScenarioManagement();
        }

        public static IServiceCollection AddSerialization(this IServiceCollection services)
        {
            return services
                .AddScoped<IPredicateClauseConverter, PredicateClauseConverter>()
                .AddScoped<IConsequenceClauseConverter, ConsequenceClauseConverter>();

        }
    }
}
