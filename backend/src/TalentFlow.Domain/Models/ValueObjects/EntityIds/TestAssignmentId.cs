using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace TalentFlow.Domain.Models.ValueObjects.EntityIds;

public class TestAssignmentId : ComparableValueObject
{
    [JsonConstructor]
    private TestAssignmentId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static TestAssignmentId NewId() => new TestAssignmentId(Guid.NewGuid());

    public static TestAssignmentId Create(Guid id) => new(id);

    public static implicit operator TestAssignmentId(Guid id) => new(id);

    public static implicit operator Guid(TestAssignmentId testAssignmentId) => testAssignmentId.Value;

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}