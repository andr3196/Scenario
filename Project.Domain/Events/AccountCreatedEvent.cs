using System;
using Scenario.Domain.SharedTypes;

namespace Project.Domain.Events
{
    public class AccountCreatedEvent : BaseEvent<Customer>
    {
        public AccountCreatedEvent(Customer customer) : base(customer)
        {
            Customer = customer;
        }

        public Customer Customer { get; }
    }
}
