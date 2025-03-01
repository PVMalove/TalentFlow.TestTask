using CSharpFunctionalExtensions;
using MediatR;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse, ErrorList>>;