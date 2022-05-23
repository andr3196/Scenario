namespace Scenario.Core.Exceptions;

public class MissingDomainAssembliesException : ScenarioException
{
    public MissingDomainAssembliesException() : base("No domain assemblies registered. Register at least 1 domain assembly by calling IScenarioConfigurationBuilder.RegisterDomainAssembly")
    {
            
    }
}