using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Core.Persistence;

namespace Scenario.Configuration;

public interface IScenarioConfigurationBuilder
{
    ScenarioBuilder RegisterDomainAssembly(Assembly assembly);
    ScenarioBuilder RegisterDomainAssembly(Type typeInAssembly);
    ScenarioBuilder RegisterDomainAssembly<TAssembly>();

    void UsePersistenceRepository<TRepository>(Action<IServiceCollection>? configureServices = null)
        where TRepository : class, IScenarioRepository;
}