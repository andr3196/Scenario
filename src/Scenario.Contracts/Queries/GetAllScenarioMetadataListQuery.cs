using MediatR;

namespace Scenario.Contracts.Queries;

public record GetAllScenarioMetadataListQuery : IRequest<ScenarioDefinitionDto[]>;