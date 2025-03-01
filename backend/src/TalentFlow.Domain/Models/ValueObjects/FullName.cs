using CSharpFunctionalExtensions;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Domain.Models.ValueObjects;

public class FullName : ComparableValueObject
{
    public string? FirstName { get; }
    public string? SecondName { get; }

    private FullName(string? firstName, string? secondName)
    {
        FirstName = firstName;
        SecondName = secondName;
    }

    public static Result<FullName, Error> Create(string? firstName, string? secondName)
    {
        return new FullName(firstName, secondName);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return FirstName ?? string.Empty;
        yield return SecondName ?? string.Empty;
    }
}