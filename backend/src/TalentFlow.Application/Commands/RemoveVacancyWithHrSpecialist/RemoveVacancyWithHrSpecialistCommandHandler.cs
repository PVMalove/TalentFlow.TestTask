using CSharpFunctionalExtensions;
using FluentValidation;
using TalentFlow.Application.Abstractions;
using TalentFlow.Application.Abstractions.Common;
using TalentFlow.Application.Abstractions.Repositories;
using TalentFlow.Application.Validation;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Application.Commands.RemoveVacancyWithHrSpecialist;

public class RemoveVacancyWithHrSpecialistCommandHandler(
    IValidator<RemoveVacancyWithHrSpecialistCommand> validator,
    IHrSpecialistRepository hrSpecialistRepository,
    IVacancyRepository vacancyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveVacancyWithHrSpecialistCommand>
{
    public async Task<UnitResult<ErrorList>> Handle(RemoveVacancyWithHrSpecialistCommand command,
        CancellationToken cancellationToken)
    {
        var validateResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validateResult.IsValid)
            return validateResult.ToList();

        var hrSpecialist = await hrSpecialistRepository.GetById(command.HrSpecialistId, cancellationToken);
        if (hrSpecialist.IsFailure)
            return hrSpecialist.Error.ToErrorList();
        
        var vacancy = await vacancyRepository.GetById(command.VacancyId, cancellationToken);
        if (vacancy.IsFailure)
            return vacancy.Error.ToErrorList();

        hrSpecialist.Value.RemoveVacancy(command.VacancyId);
        if (hrSpecialist.IsFailure)
            return hrSpecialist.Error.ToErrorList();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}