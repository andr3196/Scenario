using System;
using System.Collections.Generic;
using Project.Domain.Events;
using Scenario.Domain.Modeling.Attributes;

namespace Project.Domain
{
    [ScenarioEnabled]
    public class Customer: Entity
    {
        public Customer()
        {
        }

        public string Name { get; set; }

        [ScenarioIgnore]
        public string Password { get; set; }

        public IEnumerable<Order> Orders { get; set; }

        [ScenarioEvent(typeof(AccountCreatedEvent))]
        public void AccountClosed()
        {
            Raise(new AccountClosedEvent(this));
        }

        [ScenarioEvent(typeof(AccountCreatedEvent))]
        public void AccountCreated()
        {
            Raise(new AccountCreatedEvent(this));
        }

        [ScenarioEvent(typeof(PasswordChangedEvent))]
        public void PasswordChanged()
        {
            Raise(new PasswordChangedEvent(this));
        }
    }
}
