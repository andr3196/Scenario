using Scenario.TestDataBuilder.Domain;

namespace Scenario.Project.Test;

public static class DependencyInjection
{
    public static IServiceCollection AddScenarioTest(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient<ScenarioDefinitionBuilder>();
    }
}