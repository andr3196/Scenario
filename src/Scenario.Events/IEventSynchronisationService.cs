namespace Scenario.Events;

public interface IEventSynchronisationService
{
    Task WaitForTasksToCompleteAsync(CancellationToken cancellationToken);

    public void AddTask(Task newTask);
}