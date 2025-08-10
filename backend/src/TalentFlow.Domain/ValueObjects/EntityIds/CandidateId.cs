using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace TalentFlow.Domain.ValueObjects.EntityIds;

public class CandidateId : ComparableValueObject
{
    [JsonConstructor]
    private CandidateId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static CandidateId NewId() => new CandidateId(Guid.NewGuid());

    public static CandidateId Create(Guid id) => new(id);

    public static implicit operator CandidateId(Guid id) => new(id);

    public static implicit operator Guid(CandidateId candidateId) => candidateId.Value;

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}