using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Core.Exceptions;
using Scenario.Core.Persistence;

namespace Scenario.Configuration;

public class ScenarioBuilder : IScenarioConfigurationBuilder
{
    private List<Assembly> DomainAssemblies { get; } = new();
    private Action<IServiceCollection>? RepositoryInstaller { get; set; }

    public ScenarioBuilder RegisterDomainAssembly(Assembly assembly)
    {
        DomainAssemblies.Add(assembly);
        return this;
    }

    public ScenarioBuilder RegisterDomainAssembly(Type typeInAssembly)
    {
        return RegisterDomainAssembly(typeInAssembly.Assembly);
    }

    public ScenarioBuilder RegisterDomainAssembly<TAssembly>()
    {
        return RegisterDomainAssembly(typeof(TAssembly).Assembly);
    }

    public void UsePersistenceRepository<TRepository>(Action<IServiceCollection>? configureServices = null)
        where TRepository : class, IScenarioRepository
    {
        RepositoryInstaller = services =>
        {
            services.AddScoped<IScenarioRepository, TRepository>();
            configureServices?.Invoke(services);
        };
    }

    public ScenarioConfiguration Build()
    {
        if (!DomainAssemblies.Any())
        {
            throw new MissingDomainAssembliesException();
        }

        if (RepositoryInstaller == default)
        {
            throw new NoRepositoryConfiguredException();
        }
        
        return new ScenarioConfiguration(DomainAssemblies.ToArray(), RepositoryInstaller);
    }
}