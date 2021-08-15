using System;
using Scenario.Domain.SharedTypes;

namespace Project.Domain.Events
{
    public class EmailSentEvent : BaseEvent<Email>
    {
        public EmailSentEvent(Email email) : base(email)
        {
            Email = email;
        }

        public Email Email { get; }
    }
}
