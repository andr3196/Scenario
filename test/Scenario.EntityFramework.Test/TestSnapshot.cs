using Microsoft.Extensions.Options;

namespace Scenario.EntityFramework.Test;

public class TestSnapshot<TOptions> : IOptionsSnapshot<TOptions> where TOptions : class
{
    public TOptions Value { get; }
    public TOptions Get(string name)
    {
        return Value;
    }

    public TestSnapshot(TOptions options)
    {
        Value = options;
    }
}