using System;
using System.Threading;
using System.Threading.Tasks;

namespace Scenario.Domain.ScenarioDefinitions
{
    public delegate Task ScenarioEventHandlerAsync(object payload, CancellationToken cancellationToken);

    public delegate Task HandlerInvoker(object input, CancellationToken cancellationToken);

    public delegate Task HandlerProvider(object input, CancellationToken cancellationToken);
}
