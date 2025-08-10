using CSharpFunctionalExtensions;
using TalentFlow.Domain.Enums;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Domain.ValueObjects;

public class RecruitmentStage : ComparableValueObject
{
    public RecruitmentStageType Type { get; }
    public DateTime Date { get; }
    public StageResult Result { get; set; }

    private RecruitmentStage(RecruitmentStageType type, DateTime date, StageResult result)
    {
        Type = type;
        Date = date;
        Result = result;
    }

    public static Result<RecruitmentStage, Error> Create(RecruitmentStageType type, DateTime date, StageResult result)
    {
        if (date < DateTime.UtcNow.AddDays(-1))
            return Errors.General.ValueIsInvalid(("Stage date cannot be in the past"));

        return new RecruitmentStage(type, date, result);
    }

    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Type;
        yield return Date; 
        yield return Result;
    }
}