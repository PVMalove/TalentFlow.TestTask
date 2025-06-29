using CSharpFunctionalExtensions;
using MediatR;
using TalentFlow.Domain.Shared;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace TalentFlow.API.Extensions.New;

public static class MinimalApiResultExtensions
{
    public static IResult EndpointMatchOk<T>(
        this Result<T, ErrorList> result)
    {
        return result.Match(
            onSuccess: value => TypedResults.Ok(ApiResponse.Success(value)),
            onFailure: errors => Results.Json(
                data: ApiResponse.Failure(errors),
                statusCode: errors.GetMaxStatusCode()
            )
        );
    }

    public static IResult EndpointMatchNoContent(
        this UnitResult<ErrorList> result)
    {
        return result.Match(
            onSuccess: _ => TypedResults.NoContent(),
            onFailure: errors => Results.Json(
                data: ApiResponse.Failure(errors),
                statusCode: errors.GetMaxStatusCode()
            )
        );
    }

    private static TOut Match<TOut>(
        this UnitResult<ErrorList> result,
        Func<Unit, TOut> onSuccess,
        Func<ErrorList, TOut> onFailure)
    {
        return result.IsSuccess 
            ? onSuccess(Unit.Value) 
            : onFailure(result.Error);
    }

    private static TOut Match<T, TOut>(
        this Result<T, ErrorList> result,
        Func<T, TOut> onSuccess,
        Func<ErrorList, TOut> onFailure)
    {
        return result.IsSuccess 
            ? onSuccess(result.Value) 
            : onFailure(result.Error);
    }
}