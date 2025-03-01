using System.Reflection;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Application.Validation;

public class ResultValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest> _validator;

    public ResultValidationPipelineBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid)
            return await next();

        var errorList = CreateErrorList(validationResult);

        if (IsResultType())
            return CreateResultResponse(errorList);

        if (IsUnitResultType())
            return CreateUnitResultResponse(errorList);

        return await next();
    }

    private static ErrorList CreateErrorList(ValidationResult validationResult) =>
        new(validationResult.Errors.Select(e => Error.Deserialize(e.ErrorMessage)));

    private bool IsResultType() =>
        typeof(TResponse).IsGenericType &&
        typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<,>) &&
        typeof(TResponse).GetGenericArguments()[1] == typeof(ErrorList);

    private bool IsUnitResultType() =>
        typeof(TResponse).IsGenericType &&
        typeof(TResponse).GetGenericTypeDefinition() == typeof(UnitResult<>) &&
        typeof(TResponse).GetGenericArguments()[0] == typeof(ErrorList);

    private TResponse CreateResultResponse(ErrorList errorList)
    {
        var successType = typeof(TResponse).GetGenericArguments()[0];
        var resultType = typeof(Result<,>).MakeGenericType(successType, typeof(ErrorList));

        var constructor = resultType.GetConstructor(
                BindingFlags.NonPublic |
                BindingFlags.Instance,
                null,
                new[] { typeof(bool), typeof(ErrorList), successType },
                null)!
            .Invoke(new object[] { true, errorList, null });

        return (TResponse)constructor;
    }

    private TResponse CreateUnitResultResponse(ErrorList errorList)
    {
        var resultType = typeof(UnitResult<>).MakeGenericType(typeof(ErrorList));

        var constructor = resultType.GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            null,
            new[] { typeof(bool), typeof(ErrorList).MakeByRefType() },
            null)!;

        var result = constructor
            .Invoke(new object[] { true, errorList });

        return (TResponse)result;
    }
}