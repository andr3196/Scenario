namespace Scenario.Core.Exceptions;

public class NoRepositoryConfiguredException : ScenarioException
{
    public NoRepositoryConfiguredException() : base("No persistence repository registered. Register a repository using IScenarioConfigurationBuilder.UsePersistenceRepository")
    {
        
    }
}