namespace Scenario.Domain.Modeling.Services.Translation
{
    public class BasicTranslationService : ITranslationService
    {
        public BasicTranslationService()
        {
        }

        public string Translate(string key)
        {
            return key;
        }
    }
}
