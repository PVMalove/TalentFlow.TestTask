using CSharpFunctionalExtensions;
using TalentFlow.Domain.Models.ValueObjects;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Domain.Models.Entities;

public class HrSpecialist : Shared.Entity<HrSpecialistId>
{
    public FullName FullName { get; init; } = null!;
    public ContactInfo ContactInfo { get; init; } = null!;
    public IReadOnlyCollection<Vacancy> AssignedVacancies => _assignedVacancies.AsReadOnly();

    private readonly List<Vacancy> _assignedVacancies = [];
    
    protected HrSpecialist(HrSpecialistId id) : base(id){}

    private HrSpecialist(HrSpecialistId id,  FullName fullName, ContactInfo contactInfo) : base(id)
    {
        FullName = fullName;
        ContactInfo = contactInfo;
    }
    
    public static Result<HrSpecialist, Error> Create(HrSpecialistId id,  FullName fullName, ContactInfo contactInfo)
    {
        if (string.IsNullOrWhiteSpace(fullName.FirstName))
            return Errors.General.ValueIsInvalid("FirstName can not be empty");
        
        if (string.IsNullOrWhiteSpace(fullName.SecondName))
            return Errors.General.ValueIsInvalid("SecondName can not be empty");

        return new HrSpecialist(id, fullName, contactInfo);
    }
    
    public UnitResult<Error> AssignToVacancy(Vacancy vacancy, int maxVacancies = 5)
    {
        if (_assignedVacancies.Count >= maxVacancies)
            return Errors.General.ValueIsInvalid("HR has too many assigned vacancies");
        
        _assignedVacancies.Add(vacancy);
        return UnitResult.Success<Error>();
    }
    
    public UnitResult<Error> RemoveVacancy(VacancyId vacancyId)
    {
        var vacancy = _assignedVacancies.FirstOrDefault(v => v.Id == vacancyId);
        if (vacancy is null)
            return Errors.General.NotFound(vacancyId);
        
        _assignedVacancies.Remove(vacancy);
        return UnitResult.Success<Error>();
    }
}