using System;
using System.Collections.Generic;
using Scenario.Domain.Modeling.Models;

namespace Scenario.Domain.Modeling.Services
{
    public interface IGetEventsStrategy : IScenarioService
    {
        public IEnumerable<Event> GetEvents(Type type);
    }
}
