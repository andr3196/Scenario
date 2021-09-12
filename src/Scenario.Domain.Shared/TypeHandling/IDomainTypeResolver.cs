using System;
namespace Scenario.Domain.Shared.TypeHandling
{
    public interface IDomainTypeResolver
    {
        Type? ResolveType(string key);

        string GenerateKey(Type type);
    }
}
