using System;
using MediatR;
using Scenario.Contracts.Commands;

namespace Scenario.Application.CommandHandlers.Create;

public interface ICreateScenarioCommandHandler : IRequestHandler<CreateScenarioCommand, Guid>
{
    
}