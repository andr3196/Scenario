using Scenario.Domain.Modeling.Attributes;

namespace Scenario.Test.TypeHandling.Mocks
{
    public class MockClass1
    {
        private ClassFieldType Field;

        public ClassPropertyType PublicProperty { get; set; }

        private ClassPropertyType2 PrivateProperty { get; set; }

        [ScenarioEvent(typeof(EventType1))]
        public void PublicMethodWithAttribute() { }


        [ScenarioEvent(typeof(EventType2))]
        private void PrivateMethodWithAttribute() { }

        [ScenarioEvent(typeof(EventType3))]
        public void PublicGenericMethodWithAttribute<T>() { }
    }

    public class ClassPropertyType
    {

    }

    public class ClassPropertyType2
    {

    }

    public class ClassFieldType
    {

    }
}
