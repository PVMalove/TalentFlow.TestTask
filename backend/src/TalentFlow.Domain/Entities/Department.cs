using CSharpFunctionalExtensions;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects.EntityIds;

namespace TalentFlow.Domain.Entities;

public class Department : Shared.Entity<DepartmentId>
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public IReadOnlyCollection<Vacancy> Vacancies { get; private set; } = new List<Vacancy>();
    
    protected Department(DepartmentId id) : base(id) { }

    private Department(DepartmentId id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }

    public static Result<Department, Error> Create(DepartmentId id, string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Errors.General.ValueIsInvalid("Name can not be empty");

        if (string.IsNullOrWhiteSpace(description))
            return Errors.General.ValueIsInvalid("Description can not be empty");
        
        if (name.Length > Constants.MAX_LOW_TEXT_LENGTH_50)
            return Errors.General.ValueIsRequired("Department.Name", name.Length);

        if (description.Length > Constants.MAX_HIGH_TEXT_LENGTH_2000)
            return Errors.General.ValueIsRequired("Department.Description", description.Length);

        return new Department(id, name, description);
    }
}