using TalentFlow.Application.Commands.RemoveVacancyWithHrSpecialist;

namespace TalentFlow.API.Controllers.HrSpecialists.Request;

public record RemoveVacancyWithHrSpecialistRequest(Guid VacancyId)
{
    public RemoveVacancyWithHrSpecialistCommand ToCommand(Guid id) =>
        new(id, VacancyId);
}