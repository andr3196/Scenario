using System;
namespace Scenario.Domain.Modeling.Models.Constants
{
    public interface IConstant<T> : IConstant
    {

        T Value { get; }
    }

    public interface IConstant
    {
        string Key { get; }

        object Evaluate();
    }
}
