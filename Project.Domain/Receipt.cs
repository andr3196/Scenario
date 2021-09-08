using Project.Domain.Events;
using Scenario.Domain.Modeling.Attributes;

namespace Project.Domain
{
    [ScenarioEnabled]
    public class Receipt: Entity
    {
        public Receipt()
        {
        }

        public void Send()
        {

        }

        [ScenarioEvent(typeof(CreatedEvent<Receipt>))]
        public override void Created()
        {
            Raise(new CreatedEvent<Receipt>(this));
        }

        [ScenarioEvent(typeof(CreatedEvent<Receipt>))]
        public override void Updated()
        {
            Raise(new UpdatedEvent<Receipt>(this));
        }
    }
}