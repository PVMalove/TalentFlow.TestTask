using CSharpFunctionalExtensions;
using MediatR;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Application.Abstractions;

public interface IQueryHandler<TResponse, in TQuery> : IRequestHandler<TQuery, Result<TResponse, ErrorList>>
    where TQuery : IQuery<TResponse>;

public interface IQueryHandlerWithResult<TResponse, in TQuery> : IRequestHandler<TQuery, Result<TResponse, ErrorList>>
    where TQuery : IQuery<TResponse>;