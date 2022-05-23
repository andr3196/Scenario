using Microsoft.Extensions.DependencyInjection;

namespace Scenario.Domain.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceModel(this IServiceCollection services)
    {
        return services.AddTransient<IDatabaseScenarioMapper, DatabaseScenarioMapper>();
    }
}