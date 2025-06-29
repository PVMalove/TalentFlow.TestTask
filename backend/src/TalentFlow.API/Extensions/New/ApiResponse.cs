using TalentFlow.Domain.Shared;

namespace TalentFlow.API.Extensions.New;

public record ApiResponse
{
    public object? Result { get; init; }
    public object? Errors { get; init; }
    public DateTime TimeRequest { get; } = DateTime.UtcNow;

    public static ApiResponse Success<T>(T result) => new() { Result = result };

    public static ApiResponse Failure(ErrorList errors) => new()
    {
        Errors = errors.Select(e => new { e.Code, e.Message }).ToList()
    };
}