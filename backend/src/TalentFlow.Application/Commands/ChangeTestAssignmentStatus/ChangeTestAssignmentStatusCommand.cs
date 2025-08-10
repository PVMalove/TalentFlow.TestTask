using TalentFlow.Application.Abstractions;
using TalentFlow.Domain.Enums;
using TalentFlow.Domain.ValueObjects.EntityIds;


namespace TalentFlow.Application.Commands.ChangeTestAssignmentStatus;

public record ChangeTestAssignmentStatusCommand(
    TestAssignmentId TestAssignmentId,
    AssignmentStatus NewStatus,
    string? SubmissionUrl = null) : ICommand;