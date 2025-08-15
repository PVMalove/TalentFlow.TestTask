using System.Text.Json.Serialization;

namespace TalentFlow.E2E;

public class ApiResponse<T>
{
    public T? Result { get; init; }
    public List<ErrorResponse>? Errors { get; init; }

    [method: JsonConstructor]
    public record ErrorResponse(string Code, string Description, string Type, string? InvalidField = null);
}