using System;
using Project.Domain.Events;
using Scenario.Domain.Modeling.Attributes;

namespace Project.Domain
{
    [ScenarioEnabled]
    public class Email : Entity
    {
        public Email()
        {
        }

        [ScenarioEvent(typeof(EmailSentEvent))]
        public void Sent()
        {
            Raise(new EmailSentEvent(this));
        }

        [ScenarioEvent(typeof(CreatedEvent<Email>))]
        public override void Created()
        {
            Raise(new CreatedEvent<Email>(this));
        }

        [ScenarioEvent(typeof(CreatedEvent<Email>))]
        public override void Updated()
        {
            Raise(new UpdatedEvent<Email>(this));
        }
    }
}
