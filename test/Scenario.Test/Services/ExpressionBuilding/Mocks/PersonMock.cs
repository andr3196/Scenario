using System;
namespace Scenario.Test.Services.ExpressionBuilding.Mocks
{
    public class PersonMock
    {
        public PersonMock()
        {    
        }

        public string? Name { get; set; }

        public int? Age { get; set; }

        public DateTime? Birthday { get; set; }

        public PersonMock? Parent { get; set; }
    }
}
