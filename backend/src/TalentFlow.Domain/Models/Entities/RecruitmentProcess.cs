using CSharpFunctionalExtensions;
using TalentFlow.Domain.Models.Enum;
using TalentFlow.Domain.Models.ValueObjects;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Domain.Models.Entities;

public class RecruitmentProcess : Shared.Entity<RecruitmentProcessId>
{
    public VacancyId VacancyId { get; init; }
    public CandidateId CandidateId { get; init; }
    public TestAssignmentId? TestAssignmentId { get; init; }
    public TestAssignment? TestAssignment { get; private set; }
    public RecruitmentStageType CurrentStage { get; private set; } = RecruitmentStageType.Probation;
    public IReadOnlyList<RecruitmentStage> Stages => _stages;
    public bool ProbationPassed { get; private set; }

    private readonly List<RecruitmentStage> _stages = [];

    protected RecruitmentProcess(RecruitmentProcessId id) : base(id) { }

    private RecruitmentProcess(RecruitmentProcessId id, VacancyId vacancyId,
        CandidateId candidateId, TestAssignmentId? testAssignmentId) : base(id)
    {
        VacancyId = vacancyId;
        CandidateId = candidateId;
        TestAssignmentId = testAssignmentId;
    }

    public static Result<RecruitmentProcess, Error> Create(RecruitmentProcessId id, VacancyId vacancyId,
        CandidateId candidateId, TestAssignmentId? testAssignmentId)
    {
        return new RecruitmentProcess(id, vacancyId, candidateId, testAssignmentId);
    }

    public UnitResult<Error> AddStage(RecruitmentStage stage)
    {
        if (_stages.Any(s => s.Type == stage.Type))
            return Errors.General.AlreadyExist("Stage type already exists");

        _stages.Add(stage);
        CurrentStage = stage.Type;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> CompleteStage(RecruitmentStageType stageType, StageResult result)
    {
        var stage = _stages.FirstOrDefault(s => s.Type == stageType);

        if (stage is null)
            return Errors.General.AlreadyExist("Stage not found");

        if (stage.Result != StageResult.Pending)
            return Errors.General.ValueIsInvalid("Stage already completed");

        stage.Result = result;
        _stages.RemoveAll(s => s.Type == stageType);
        _stages.Add(stage);

        if (stageType == RecruitmentStageType.Probation && result == StageResult.Passed)
            ProbationPassed = true;

        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> AssignTestAssignment(TestAssignment testAssignment)
    {
        if (TestAssignment != null)
            return Errors.General.ValueIsInvalid("Test assignment is already assigned");

        TestAssignment = testAssignment;
        return UnitResult.Success<Error>();
    }
}