using TalentFlow.Domain.Shared;

namespace TalentFlow.Domain.DomainError;

public static class DomainErrors
{
    public static Error ValueIsInvalid(string? name = null)
    {
        var label = name ?? "value";
        return Error.Validation(
            "DomainErrors.ValueIsInvalid",
            $"{label} недействителен",
            nameof(label));
    }
    
    public static Error ValueIsLengthInvalid(string? name = null, int validLength = 0, int invalidLength = 0)
    {
        var label = name == null ? "" : " " + name + " ";;
        return Error.Validation(
            "DomainErrors.ValueIsLengthInvalid",
            $"{label} имеет недопустимую длину: {invalidLength}. Допустимая длина VIN - {validLength} символов.",
            nameof(label));
    }
}