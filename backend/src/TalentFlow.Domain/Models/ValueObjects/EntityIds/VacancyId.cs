using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace TalentFlow.Domain.Models.ValueObjects.EntityIds;

public class VacancyId : ComparableValueObject
{
    [JsonConstructor]
    private VacancyId(Guid value)
    {
        Value = value;
    }
    
    public Guid Value { get; }
   
    public static VacancyId NewId() => new VacancyId(Guid.NewGuid());
    
    public static VacancyId Create(Guid id) => new(id);
    
    public static implicit operator VacancyId(Guid id) => new(id);

    public static implicit operator Guid(VacancyId vacancyId)
    {
        ArgumentNullException.ThrowIfNull(vacancyId);
        return vacancyId.Value;
    }
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}