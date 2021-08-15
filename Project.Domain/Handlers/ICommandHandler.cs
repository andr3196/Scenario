using System;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Domain.Handlers
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {

        Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }

    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand
    {

        Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
    }
}
