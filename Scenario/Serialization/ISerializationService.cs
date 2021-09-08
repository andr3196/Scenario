using System;
namespace Scenario.Serialization
{
    public interface ISerializationService
    {
        string Serialize<TType>(TType type);

        TType? Deserialize<TType>(string type);
    }
}
