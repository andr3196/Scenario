using System;
namespace Scenario.Test.Services.ExpressionBuilding.Mocks
{
    public class CommandMock
    {
        public CommandMock()
        {
        }

        public string AStringProperty { get; set; }

        public DateTime ADateTimeProperty { get; set; }

        public int AnIntProperty { get; set; }

        public long ALongProperty { get; set; }

        public Guid AGuidProperty { get; set; }

        public bool ABoolProperty { get; set; }

        public double ADoubleProperty { get; set; }

        public string ANullProperty { get; set; }
    }
}
