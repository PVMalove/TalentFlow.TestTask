using FluentValidation;
using TalentFlow.Application.Validation;
using TalentFlow.Domain.Shared;


namespace TalentFlow.Application.Commands.RemoveVacancyWithHrSpecialist;

public class RemoveVacancyWithHrSpecialistCommandValidator : AbstractValidator<RemoveVacancyWithHrSpecialistCommand>
{
    public RemoveVacancyWithHrSpecialistCommandValidator()
    {
        RuleFor(x => x.VacancyId)
            .NotNull()
            .WithError(Errors.General.Null());
    }
}