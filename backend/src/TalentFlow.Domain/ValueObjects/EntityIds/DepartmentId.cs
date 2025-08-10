using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace TalentFlow.Domain.ValueObjects.EntityIds;

public class DepartmentId : ComparableValueObject
{
    [JsonConstructor]
    private DepartmentId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static DepartmentId NewId() => new DepartmentId(Guid.NewGuid());
    
    public static DepartmentId Create(Guid id) => new(id);

    public static implicit operator DepartmentId(Guid id) => new(id);

    public static implicit operator Guid(DepartmentId departmentId) => departmentId.Value;

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}