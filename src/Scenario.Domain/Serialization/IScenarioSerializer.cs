namespace Scenario.Domain
{
    public interface IScenarioSerializer
    {
        string Serialize<TType>(TType type);

        TType? Deserialize<TType>(string type);
    }
}
