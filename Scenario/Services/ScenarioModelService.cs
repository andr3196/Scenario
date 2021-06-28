using System;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.Modeling;

namespace Scenario.Services
{
    public class ScenarioModelService: IScenarioModelService
    {
        public ScenarioModelService()
        {
        }

        public Task<Node> GetModel(CancellationToken cancellationToken)
        {
            return Task.FromResult(new Node());
        }
    }
}
