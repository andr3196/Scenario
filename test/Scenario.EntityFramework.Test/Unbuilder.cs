namespace Scenario.EntityFramework.Test;

public class Unbuilder<TEntity> : IAsyncDisposable
{
    public List<TEntity> Entities { get; }
    private readonly Func<Task> undo;

    public Unbuilder(List<TEntity> entities, Func<Task> undo)
    {
        Entities = entities;
        this.undo = undo;
    }

    public async ValueTask DisposeAsync()
    {
        await undo();
    }
}