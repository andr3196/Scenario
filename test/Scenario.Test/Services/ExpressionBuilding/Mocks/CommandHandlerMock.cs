using System.Threading;
using System.Threading.Tasks;

namespace Scenario.Test.Services.ExpressionBuilding.Mocks
{
    public class CommandHandlerMock
    {
        public CommandHandlerMock()
        {
        }

        public virtual Task HandleAsync(CommandMock command, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
