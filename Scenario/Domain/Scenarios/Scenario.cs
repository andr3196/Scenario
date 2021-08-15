using System;
namespace Scenario.Domain.Scenarios
{
    public class Scenario
    {
        public Scenario()
        {
            
        }

        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public bool Active { get; set; }
    }
}
