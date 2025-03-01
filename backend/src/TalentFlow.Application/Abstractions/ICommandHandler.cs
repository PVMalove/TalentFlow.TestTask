using CSharpFunctionalExtensions;
using MediatR;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Application.Abstractions;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, UnitResult<ErrorList>>
    where TCommand : ICommand;

public interface ICommandHandler<TResponse, in TCommand> : IRequestHandler<TCommand, Result<TResponse, ErrorList>>
    where TCommand : ICommand<TResponse>;