using System;
namespace Scenario.Persistence.Models
{
    public class DatabaseScenario
    {
        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        public string Title { get; set; }

        public string EventTypeString { get; set; }

        public string ConditionJson { get; set; }

        public string ConsequenceJson { get; set; }
     }
}
