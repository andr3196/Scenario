using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Scenario.Domain.Shared.TypeHandling;
using Scenario.Domain.Modeling.Attributes;
using Scenario.Domain.Extensions;

namespace Scenario.Domain.TypeHandling
{
    public class DomainTypeResolver : IDomainTypeResolver, IDisposable
    {
        private readonly Dictionary<string, Type> typeDictionary = new Dictionary<string, Type>();
        private readonly ITypeKeyGenerator typeKeyGenerator;

        public DomainTypeResolver(ITypeKeyGenerator typeKeyGenerator)
        {
            this.typeKeyGenerator = typeKeyGenerator;
        }


        public Type? ResolveType(string key)
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
                    .Where(t => !t.IsInterface && !t.IsGenericTypeDefinition)
                    .SelectMany(t => t
                        .GetProperties(
                            BindingFlags.Instance
                            | BindingFlags.Public
                            | BindingFlags.FlattenHierarchy)
                        .Select(p => p.PropertyType)
                        .Where(p => !p.IsGenericTypeParameter)
                        .AppendIf(t, !t.IsAbstract)
                        .Union(
                            t.GetMethods(
                                BindingFlags.Instance
                                | BindingFlags.Public
                                | BindingFlags.FlattenHierarchy)
                            .Where(m => Attribute.IsDefined(m, typeof(ScenarioEventAttribute)))
                            .Select(m => m.GetCustomAttributes(true).OfType<ScenarioEventAttribute>().First().EventType)))
                .Distinct()
                .OrderBy(v => v.Name)
                .ToList();
            x.ForEach(t => typeDictionary.Add(GenerateKey(t), t));
            foreach (var (key, val) in this.typeDictionary.OrderBy(p => p.Value.Name))
            {
                Console.WriteLine($"Key: {key}, Val: {val}");
            }
       } 

        public string GenerateKey(Type type)
        {
            return typeKeyGenerator.GenerateKey(type);
        }

        public void Dispose()
        {
            typeKeyGenerator.Dispose();
        }
    }
}
