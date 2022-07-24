namespace Scenario.Contracts.Exceptions.Tokenizers
{
    public class ScenarioTokenizationRepeatedKeywordException : Exception
    {
        public ScenarioTokenizationRepeatedKeywordException(string keyword) : base($"Keyword '{keyword}' is repeated more times than allowed")
        {
            
        }
    }
}