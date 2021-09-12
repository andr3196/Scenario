using System;
using System.Security.Cryptography;
using Scenario.Domain.Shared.TypeHandling;

namespace Scenario.Domain.TypeHandling
{
    public class TypeKeyGenerator : ITypeKeyGenerator
    {
        private readonly SHA256 sha = new SHA256Managed();

        public void Dispose() => sha.Dispose();

        public string GenerateKey(Type type)
        {
            byte[] textData = System.Text.Encoding.UTF8.GetBytes(type.FullName ?? type.Name);
            byte[] hash = sha.ComputeHash(textData);
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}
