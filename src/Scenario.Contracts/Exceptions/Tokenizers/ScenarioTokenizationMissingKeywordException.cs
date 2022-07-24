namespace Scenario.Contracts.Exceptions.Tokenizers
{
    public class ScenarioTokenizationMissingKeywordException : Exception
    {
        public ScenarioTokenizationMissingKeywordException(string keyword) : base($"Scenario definition must contain keyword '{keyword}'")
        {
            
        }
    }
}