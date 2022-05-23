using MediatR;

namespace Scenario.Contracts.Queries;

public record GetAllScenariosQuery : IRequest<ScenarioDefinitionDto[]>;