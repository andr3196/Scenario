using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Scenario.Configuration;

public class ScenarioConfiguration
{
    public Assembly[] DomainAssemblies { get; }
    
    public Action<IServiceCollection> RepositoryInstaller { get; }

    public ScenarioConfiguration(Assembly[] domainAssemblies, Action<IServiceCollection> repositoryInstaller)
    {
        DomainAssemblies = domainAssemblies;
        RepositoryInstaller = repositoryInstaller;
    }
}