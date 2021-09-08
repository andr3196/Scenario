using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Scenario.Domain.SharedTypes;
using System.Security.Cryptography;
using Scenario.Domain.Modeling.Attributes;

namespace Scenario.Services
{
    public class DomainTypeResolver : IDomainTypeResolver, IDisposable
    {
        private readonly SHA256 sha = new SHA256Managed();

        private readonly Dictionary<string, Type> typeDictionary = new Dictionary<string, Type>();

        public Type? ResolveTypeFromKey(string key)
        {
            if (key == null)
            {
                return null;
            }

            typeDictionary.TryGetValue(key, out var type);
            return type;
        }

        public void RegisterAllTypesFromAssembly(Assembly assembly)
        {
            var x = assembly
                    .GetTypes()
                    .Where(t => !t.IsInterface && !t.IsAbstract && !t.IsGenericTypeDefinition)
                    .SelectMany(t => t
                        .GetProperties(System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.Public
                    | System.Reflection.BindingFlags.FlattenHierarchy)
                        .Select(p => p.PropertyType).Where(p => !p.IsGenericTypeParameter).Append(t)
                        .Union(t
                        .GetMethods(
                            System.Reflection.BindingFlags.Instance
                            | System.Reflection.BindingFlags.Public
                            | System.Reflection.BindingFlags.FlattenHierarchy)
                .Where(m => Attribute.IsDefined(m, typeof(ScenarioEventAttribute)))
                .Select(m => m.GetCustomAttributes(true).OfType<ScenarioEventAttribute>().First().EventType)))
                    .Distinct()
                    .OrderBy(v => v.Name)
                    .ToList();
                    x.ForEach(t => typeDictionary.Add(GetKey(t), t));
            foreach (var (key, val) in this.typeDictionary.OrderBy(p => p.Value.Name))
            {
                Console.WriteLine($"Key: {key}, Val: {val}");
            }
       } 

        public string GetKey(Type type)
        {
            byte[] textData = System.Text.Encoding.UTF8.GetBytes(type.FullName);
            byte[] hash = sha.ComputeHash(textData);
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }

        public void Dispose()
        {
            sha.Dispose();
        }
    }
}
