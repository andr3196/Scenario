using Scenario.Core;
using Scenario.Core.Persistence;
using Scenario.Domain.Models;
using Scenario.Domain.Persistence;

namespace Scenario.EntityFrameworkCore;

public class ScenarioRepository : IScenarioRepository
{
    private readonly IRepository<DatabaseScenario> repository;
    private readonly IDatabaseScenarioMapper mapper;

    public ScenarioRepository(IRepository<DatabaseScenario> repository, IDatabaseScenarioMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }
    
    public async Task<IEnumerable<ScenarioFlow>> GetAllActiveScenarioDefinitionsAsync(CancellationToken cancellationToken)
    {
        return (await repository.GetAll(cancellationToken))
            .Select(mapper.Map);
    }
}