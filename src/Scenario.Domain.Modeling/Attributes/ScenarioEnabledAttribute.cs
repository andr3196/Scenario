using System;
namespace Scenario.Domain.Modeling.Attributes
{
    public class ScenarioEnabledAttribute: Attribute
    {
        public ScenarioEnabledAttribute(string label = null)
        {
            Label = label;
        }

        public string Label { get; }
    }
}
