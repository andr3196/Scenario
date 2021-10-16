using System;
namespace Scenario.Domain.Exceptions
{
    public class ExpressionBuildingException : Exception 
    {
        public ExpressionBuildingException(string message) : base(message)
        {
        }
    }
}
