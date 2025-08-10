using CSharpFunctionalExtensions;
using TalentFlow.Application.Abstractions;
using TalentFlow.Application.Abstractions.Common;
using TalentFlow.Domain.Abstractions.Repositories;
using TalentFlow.Domain.Enums;
using TalentFlow.Domain.Shared;


namespace TalentFlow.Application.Commands.ChangeTestAssignmentStatus;

public class ChangeTestAssignmentStatusCommandHandler(
    ITestAssignmentRepository testAssigmentRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<ChangeTestAssignmentStatusCommand>
{
    public async Task<UnitResult<ErrorList>> Handle(ChangeTestAssignmentStatusCommand request,
        CancellationToken cancellationToken)
    {
        var testAssignment = await testAssigmentRepository.GetById(request.TestAssignmentId, cancellationToken);
        if (testAssignment.IsFailure)
            return testAssignment.Error.ToErrorList();

        var result = request.NewStatus switch
        {
            AssignmentStatus.Submitted => testAssignment.Value.Submit(request.SubmissionUrl!),
            AssignmentStatus.Approved => testAssignment.Value.Approve(),
            AssignmentStatus.Rejected => testAssignment.Value.Reject(),
            _ => Errors.General.ValueIsInvalid("Invalid status")
        };
        if (result.IsFailure)
            return result.Error.ToErrorList();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}