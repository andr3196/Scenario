namespace Scenario.Domain.Modeling.Services.Translation
{
    public interface ITranslationService : IScenarioService
    {
        string Translate(string key);
    }
}
