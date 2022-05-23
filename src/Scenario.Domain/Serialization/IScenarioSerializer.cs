namespace Scenario.Domain.Serialization
{
    public interface IScenarioSerializer
    {
        string Serialize<TType>(TType type);

        TType? Deserialize<TType>(string type);
    }
}
