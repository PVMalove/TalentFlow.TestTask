using CSharpFunctionalExtensions;
using FluentValidation;
using TalentFlow.Application.Abstractions;
using TalentFlow.Application.Abstractions.Common;
using TalentFlow.Application.Validation;
using TalentFlow.Domain.Abstractions.Repositories;
using TalentFlow.Domain.DTO.RecruitmentProcess;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Enums;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects;
using TalentFlow.Domain.ValueObjects.EntityIds;


namespace TalentFlow.Application.Commands.CreateRecruitmentProcess;

public class CreateRecruitmentProcessCommandHandler(
    IValidator<CreateRecruitmentProcessCommand> validator,
    IRecruitmentProcessRepository recruitmentProcessRepository,
    IVacancyRepository vacancyRepository,
    ICandidateRepository candidateRepository,
    ITestAssignmentRepository testAssigmentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RecruitmentProcessDto, CreateRecruitmentProcessCommand>
{
    public async Task<Result<RecruitmentProcessDto, ErrorList>> Handle(CreateRecruitmentProcessCommand command,
        CancellationToken cancellationToken)
    {
        var validateResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validateResult.IsValid)
            return validateResult.ToList();
        
        var vacancyResult = await vacancyRepository.GetById(command.VacancyId, cancellationToken);
        if (vacancyResult.IsFailure)
            return vacancyResult.Error.ToErrorList();

        var candidateResult = await candidateRepository.GetById(command.CandidateId, cancellationToken);
        if (candidateResult.IsFailure)
            return candidateResult.Error.ToErrorList();

        Result<RecruitmentStage, Error> stage;
        TestAssignmentId? testAssignmentId = null;

        if (command.TestAssignment is not null)
        {
            stage = RecruitmentStage.Create(RecruitmentStageType.TestAssignment, DateTime.UtcNow,
                StageResult.Pending);
            if (stage.IsFailure)
                return stage.Error.ToErrorList();

            testAssignmentId = TestAssignmentId.NewId();
            var testAssignment = TestAssignment.Create(testAssignmentId, command.TestAssignment.Description,
                command.TestAssignment.SubmissionDeadline);
            if (testAssignment.IsFailure)
                return stage.Error.ToErrorList();

            await testAssigmentRepository.Add(testAssignment.Value, cancellationToken);
        }
        else
        {
            stage = RecruitmentStage.Create(RecruitmentStageType.Probation, DateTime.UtcNow, StageResult.Pending);
            if (stage.IsFailure)
                return stage.Error.ToErrorList();
        }

        var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var id = RecruitmentProcessId.NewId();

            var recruitmentProcess =
                RecruitmentProcess.Create(id, command.VacancyId, command.CandidateId, testAssignmentId);
            if (recruitmentProcess.IsFailure)
                return recruitmentProcess.Error.ToErrorList();

            recruitmentProcess.Value.AddStage(stage.Value);

            await recruitmentProcessRepository.Add(recruitmentProcess.Value, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            transaction.Commit();

            var result = new RecruitmentProcessDto(recruitmentProcess.Value.CurrentStage);

            return result;
        }
        catch (Exception e)
        {
            transaction.Rollback();
            return Error.Failure($"failed to create recruitment process for vacancy {command.VacancyId.ToString()}",
                e.Message).ToErrorList();
        }
    }
}