using System;
using System.Collections.Generic;
using Project.Domain.Events;
using Scenario.Domain.Modeling.Attributes;
using Scenario.Domain.SharedTypes;

namespace Project.Domain
{
    public class Entity : IScenarioEntity
    {
        public ICollection<IDomainEvent> Events { get; } = new List<IDomainEvent>();

        public Entity()
        {
        }

        public long Id { get; set; }

        [ScenarioEvent(typeof(CreatedEvent<>))]
        public void Created()
        {
            Raise(new CreatedEvent<Entity>(this));
        }

        [ScenarioEvent(typeof(CreatedEvent<>))]
        public void Updated()
        {
            Raise(new UpdatedEvent<Entity>(this));
        }

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
