using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace TalentFlow.Domain.ValueObjects.EntityIds;

public class RecruitmentProcessId : ComparableValueObject
{
    [JsonConstructor]
    private RecruitmentProcessId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static RecruitmentProcessId NewId() => new RecruitmentProcessId(Guid.NewGuid());

    public static RecruitmentProcessId Create(Guid id) => new(id);

    public static implicit operator RecruitmentProcessId(Guid id) => new(id);

    public static implicit operator Guid(RecruitmentProcessId recruitmentProcessId) => recruitmentProcessId.Value;

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}