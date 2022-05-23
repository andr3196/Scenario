using AutoMapper;
using MediatR;
using Scenario.Contracts.Commands;

namespace Scenario.Application.CommandHandlers.Delete;

public interface IDeleteScenarioCommandHandler : IRequestHandler<DeleteScenarioCommand>
{
    
}