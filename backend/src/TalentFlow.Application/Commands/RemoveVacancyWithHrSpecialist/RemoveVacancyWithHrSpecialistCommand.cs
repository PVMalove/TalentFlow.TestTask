using TalentFlow.Application.Abstractions;

namespace TalentFlow.Application.Commands.RemoveVacancyWithHrSpecialist;

public record RemoveVacancyWithHrSpecialistCommand(
    Guid HrSpecialistId, Guid VacancyId) : ICommand;