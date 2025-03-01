using CSharpFunctionalExtensions;
using MediatR;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Application.Abstractions;

public interface ICommand : IRequest<UnitResult<ErrorList>>, IBaseCommand;

public interface ICommand<TResponse> : IRequest<Result<TResponse, ErrorList>>, IBaseCommand;

public interface IBaseCommand;
