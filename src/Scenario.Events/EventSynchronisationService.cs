using System.Collections.Concurrent;

namespace Scenario.Events;

public class EventSynchronisationService : IEventSynchronisationService
{
    private readonly ConcurrentDictionary<Task, Task> tasks = new();
    public async Task WaitForTasksToCompleteAsync(CancellationToken cancellationToken)
    {
        await Task.WhenAll(tasks.Values.ToArray());
    }

    public void AddTask(Task newTask)
    {
        tasks.TryAdd(newTask, newTask.ContinueWith(t => tasks.TryRemove(t, out _)));
    }
}