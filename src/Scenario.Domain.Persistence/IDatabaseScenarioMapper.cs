using Scenario.Domain.Models;

namespace Scenario.Domain.Persistence;

public interface IDatabaseScenarioMapper
{
    public ScenarioFlow Map(DatabaseScenario dbScenario);

    public DatabaseScenario Map(ScenarioFlow flow);
}