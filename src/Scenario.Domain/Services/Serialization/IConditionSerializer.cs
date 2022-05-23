using Scenario.Domain.Models;

namespace Scenario.Domain.Services.Serialization;

public interface IConditionSerializer
{
    public Condition Deserialize(string conditionJson);
}

public class ConditionSerializer : IConditionSerializer
{
    public Condition Deserialize(string conditionJson)
    {
        return new Condition();
    }
}