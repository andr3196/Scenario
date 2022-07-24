namespace Scenario.Contracts.Exceptions.Tokenizers
{
    public class ScenarioTokenizationBadFormatException : Exception
    {
        public ScenarioTokenizationBadFormatException(int? atPosition = null) : base($"Invalid format in scenario definition" + (atPosition.HasValue ? " near index {atPosition}" : ""))
        {
            
        }
    }
}