using System;

namespace Scenario.Domain.Shared.TypeHandling
{
    public interface ITypeKeyGenerator : IDisposable
    {
        string GenerateKey(Type type);
    }
}
