using Scenario.Domain.Modeling.Attributes;

namespace Scenario.Test.TypeHandling.Mocks
{
    public abstract class MockAbstractClass
    {
        private AbstractClassFieldType Field;

        public AbstractClassPropertyType PublicProperty { get; set; }

        private AbstractClassPropertyType2 PrivateProperty { get; set; }

        [ScenarioEvent(typeof(EventType1))]
        public void PublicMethodWithAttribute() { }


        [ScenarioEvent(typeof(EventType2))]
        private void PrivateMethodWithAttribute() { }

        [ScenarioEvent(typeof(EventType3))]
        public void PublicGenericMethodWithAttribute<T>() { }
    }

    public class AbstractClassPropertyType
    {

    }

    public class AbstractClassPropertyType2
    {

    }

    public class AbstractClassFieldType
    {

    }

    public class EventType1
    {

    }

    public class EventType2
    {

    }

    public class EventType3
    {

    }


}
