using System;
namespace Scenario.Domain.Modeling.Attributes
{
    public class ScenarioConsequenceAttribute: Attribute
    {
        public ScenarioConsequenceAttribute(string label, Type parametersType)
        {
            Label = label;
            ParametersType = parametersType;
        }

        public string Label { get; }

        public Type ParametersType { get; }
    }
}
