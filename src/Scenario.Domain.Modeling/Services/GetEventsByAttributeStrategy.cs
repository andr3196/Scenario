using System;
using System.Collections.Generic;
using System.Linq;
using Scenario.Domain.Modeling.Attributes;
using Scenario.Domain.Modeling.Models;
using Scenario.Domain.Shared.TypeHandling;

namespace Scenario.Domain.Modeling.Services
{
    public class GetEventsByAttributeStrategy : IGetEventsStrategy
    {
        private const string EventType = "event";
        private readonly IDomainTypeResolver domainTypeResolver;

        public GetEventsByAttributeStrategy(IDomainTypeResolver domainTypeResolver)
        {
            this.domainTypeResolver = domainTypeResolver;
        }

        public IEnumerable<Event> GetEvents(Type type)
        {
            return type
                .GetMethods(
                    System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.Public
                    | System.Reflection.BindingFlags.FlattenHierarchy)
                .Where(m => Attribute.IsDefined(m, typeof(ScenarioEventAttribute)))
                .Select(m => new
                {
                    Method = m,
                    Attribute = m.GetCustomAttributes(true).OfType<ScenarioEventAttribute>().First(),
                })
                .Select(m => new Event
                {
                    Label = m.Attribute.Label ?? m.Method.Name,
                    Value = domainTypeResolver.GenerateKey(m.Attribute.EventType),
                    Type = EventType,
                });
        }
    }
}
