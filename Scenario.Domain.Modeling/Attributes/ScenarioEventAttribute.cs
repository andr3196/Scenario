using System;
using Scenario.Domain.SharedTypes;

namespace Scenario.Domain.Modeling.Attributes
{
    public class ScenarioEventAttribute : Attribute
    {
        public ScenarioEventAttribute(Type eventType, string label = null)
        {
            EventType = eventType;
            Label = label;
        }

        public Type EventType { get; }
        public string Label { get; }
    }
}
