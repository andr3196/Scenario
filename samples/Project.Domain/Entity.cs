using System;
using System.Collections.Generic;
using Project.Domain.Events;
using Scenario.Domain.Modeling.Attributes;
using Scenario.Domain.SharedTypes;

namespace Project.Domain
{
    public abstract class Entity : IScenarioEntity
    {
        public ICollection<IDomainEvent> Events { get; } = new List<IDomainEvent>();

        public long Id { get; set; }

        //[ScenarioEvent(typeof(CreatedEvent<>))]
        public abstract void Created();

        //[ScenarioEvent(typeof(CreatedEvent<>))]
        public abstract void Updated();

        protected void Raise(IDomainEvent domainEvent)
        {
            Events.Add(domainEvent);
        }

        public void ResetEvents()
        {
            Events.Clear();
        }
    }
}
