using CSharpFunctionalExtensions;
using TalentFlow.Domain.Models.Enum;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Domain.Models.Entities;

public class Vacancy : Shared.Entity<VacancyId>
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public VacancyStatus Status { get; private set; } = VacancyStatus.Open;
    public DepartmentId DepartmentId { get; init; }
    public HrSpecialistId HrSpecialistId { get; init; }
    public DateTime OpeningDate { get; } = DateTime.UtcNow;
    public DateTime? ClosingDate { get; private set; }

    protected Vacancy(VacancyId id) : base(id) { }

    private Vacancy(VacancyId id, DepartmentId departmentId, HrSpecialistId hrSpecialistId,
        string title, string description) : base(id)
    {
        DepartmentId = departmentId;
        HrSpecialistId = hrSpecialistId;
        Title = title;
        Description = description;
    }

    public static Result<Vacancy, Error> Create(VacancyId id, DepartmentId departmentId, HrSpecialistId hrSpecialistId,
        string title, string description)
    {
        return new Vacancy(id, departmentId, hrSpecialistId, title, description);
    }

    public UnitResult<Error> Close(bool probationPassed)
    {
        if (!probationPassed)
            return Errors.General.ValueIsInvalid("Cannot close vacancy without passed probation");

        Status = VacancyStatus.Closed;
        ClosingDate = DateTime.UtcNow;

        return UnitResult.Success<Error>();
    }
}