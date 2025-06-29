using TalentFlow.Domain.Shared;

namespace TalentFlow.API.Extensions.New;

internal static class ErrorListExtensions
{
    public static int GetMaxStatusCode(this ErrorList errors) =>
        errors.Select(e => e.Type)
            .Distinct()
            .Select(ToStatusCode)
            .DefaultIfEmpty(StatusCodes.Status500InternalServerError)
            .Max();

    private static int ToStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };
}