using System;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.Modeling;

namespace Scenario.Services
{
    public interface IScenarioModelService
    {
        Task<Node> GetModel(CancellationToken cancellationToken);
    }
}
