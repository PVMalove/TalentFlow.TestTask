using TalentFlow.Application.Commands.ChangeTestAssignmentStatus;
using TalentFlow.Domain.Models.Enum;

namespace TalentFlow.API.Controllers.TestAssignment.Request;

public record ChangeTestAssignmentStatusRequest(
    AssignmentStatus NewStatus,
    string? SubmissionUrl = null)
{
    public ChangeTestAssignmentStatusCommand ToCommand(Guid id) =>
        new(id, NewStatus, SubmissionUrl);
}