using TalentFlow.Application.Abstractions;
using TalentFlow.Domain.Models.Enum;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;


namespace TalentFlow.Application.Commands.ChangeTestAssignmentStatus;

public record ChangeTestAssignmentStatusCommand(
    TestAssignmentId TestAssignmentId,
    AssignmentStatus NewStatus,
    string? SubmissionUrl = null) : ICommand;