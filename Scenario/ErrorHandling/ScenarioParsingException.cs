using System;
namespace Scenario.ErrorHandling
{
    public class ScenarioParsingException : FormatException
    {
        public ScenarioParsingException()
        {
        }

        public ScenarioParsingException(string message)
            :base(message)
        {
        }

        public ScenarioParsingException(string message, Exception innerException)
            :base(message, innerException)
        {

        }
    }
}
