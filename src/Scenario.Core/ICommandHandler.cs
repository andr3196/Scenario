namespace Scenario.Core;

public interface ICommandHandler<in T>
{
    ValueTask Handle(T command, CancellationToken cancellationToken);
}