using System;
namespace Scenario.Domain.SharedTypes
{
    public interface IDomainTypeResolver
    {
        Type? ResolveTypeFromKey(string key);

        string GetKey(Type type);
    }
}
