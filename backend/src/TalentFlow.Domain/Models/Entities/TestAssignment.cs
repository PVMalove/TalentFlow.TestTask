using CSharpFunctionalExtensions;
using TalentFlow.Domain.Models.Enum;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Domain.Models.Entities;

public class TestAssignment : Shared.Entity<TestAssignmentId>
{
    public string Description { get; } = null!;
    public DateTime AssignedDate { get; private set; }
    public DateTime? SubmissionDeadline { get; private set; }
    public AssignmentStatus Status { get; private set; } = AssignmentStatus.Pending;
    public string? SubmissionUrl { get; private set; }

    protected TestAssignment(TestAssignmentId id) : base(id) { }

    private TestAssignment(TestAssignmentId id, string description,
        DateTime assignedDate, DateTime? submissionDeadline) : base(id)
    {
        Description = description;
        AssignedDate = assignedDate;
        SubmissionDeadline = submissionDeadline;
    }

    public static Result<TestAssignment, Error> Create(TestAssignmentId id, string description,
        DateTime? submissionDeadline)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsInvalid("Description cannot be empty");
        if (submissionDeadline.HasValue && submissionDeadline.Value < DateTime.UtcNow)
            return Errors.General.ValueIsInvalid("Submission deadline cannot be in the past");

        return new TestAssignment(id, description, DateTime.UtcNow, submissionDeadline);
    }

    public UnitResult<Error> Submit(string submissionUrl)
    {
        if (Status != AssignmentStatus.Pending)
            return Errors.General.ValueIsInvalid("Assignment is already submitted or rejected");
        if (SubmissionDeadline.HasValue && SubmissionDeadline.Value < DateTime.UtcNow)
            return Errors.General.ValueIsInvalid("Submission deadline has passed");

        SubmissionUrl = submissionUrl;
        Status = AssignmentStatus.Submitted;
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Approve()
    {
        if (Status != AssignmentStatus.Submitted)
            return Errors.General.ValueIsInvalid("Assignment must be submitted before approval");

        Status = AssignmentStatus.Approved;
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> Reject()
    {
        if (Status != AssignmentStatus.Submitted)
            return Errors.General.ValueIsInvalid("Assignment must be submitted before rejection");

        Status = AssignmentStatus.Rejected;
        return UnitResult.Success<Error>();
    }
}