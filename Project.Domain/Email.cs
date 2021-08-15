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
    }
}
