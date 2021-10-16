namespace Scenario.Domain.Modeling.Models.Logicals
{
    public interface ILogical
    {
        string Key { get; }

        bool Apply(bool value1, bool value2);
    }
}
