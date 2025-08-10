using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace TalentFlow.Domain.ValueObjects.EntityIds;

public class HrSpecialistId : ComparableValueObject
{
    [JsonConstructor]
    private HrSpecialistId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static HrSpecialistId NewId() => new HrSpecialistId(Guid.NewGuid());

    public static HrSpecialistId Create(Guid id) => new(id);

    public static implicit operator HrSpecialistId(Guid id) => new(id);

    public static implicit operator Guid(HrSpecialistId hrSpecialistId) => hrSpecialistId.Value;

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}