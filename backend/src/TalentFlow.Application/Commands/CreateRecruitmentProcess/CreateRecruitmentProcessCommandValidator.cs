using FluentValidation;
using TalentFlow.Application.Validation;
using TalentFlow.Domain.Shared;


namespace TalentFlow.Application.Commands.CreateRecruitmentProcess;

public class CreateRecruitmentProcessCommandValidator : AbstractValidator<CreateRecruitmentProcessCommand>
{
    public CreateRecruitmentProcessCommandValidator()
    {
        RuleFor(x => x.VacancyId)
            .NotNull()
            .WithError(Errors.General.Null());

        RuleFor(x => x.CandidateId)
            .NotNull()
            .WithError(Errors.General.Null());
    }
}