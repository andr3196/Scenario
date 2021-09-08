using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.SharedTypes;

namespace Scenario.Domain.ScenarioDefinitions
{
    public class ScenarioDefinition<TEntity> : ScenarioDefinition
    {
        public Func<TEntity, bool> Condition { get; set; }

        public Func<TEntity, CancellationToken, Task> Handler { get; set; }

        public override void Install(object inputCondition, object inputConsequence)
        {
            if(inputCondition is Func<TEntity, bool> condition)
            {
                Condition = condition;
            }

            if (inputCondition is Func<TEntity, CancellationToken, Task> consequence)
            {
                Handler = consequence;
            }
        }

        public override bool InvokeCondition(object input)
        {
            if(input is TEntity entity)
            {
                return Condition(entity);
            }
            throw new ArgumentException($"Input of type {input?.GetType()} is not compatible with Definition of type {typeof(TEntity)}");
        }

        public override Task InvokeConsequence(object input, CancellationToken cancellationToken)
        {
            if(input is TEntity entity)
            {
                return Handler(entity, cancellationToken);
            }
            throw new ArgumentException($"Input of type {input?.GetType()} is not compatible with Definition of type {typeof(TEntity)}");
        }
    }


    public abstract class ScenarioDefinition
    {

        public string Key { get; set; }

        public Type EventType { get; set; }

        

        public IEnumerable<string> IncludedProperties { get; set; }



        public abstract bool InvokeCondition(object input);

        public abstract Task InvokeConsequence(object input, CancellationToken cancellationToken);

        public abstract void Install(object condition, object consequence);
    }
}
