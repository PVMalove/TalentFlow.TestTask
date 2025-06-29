using CSharpFunctionalExtensions;
using TalentFlow.Domain.DomainError;
using TalentFlow.Domain.DomainError.Departments;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;


namespace TalentFlow.Domain.Models.Entities;

public class Department : Shared.Entity<DepartmentId>
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    protected Department(DepartmentId id) : base(id)
    {
    }

    private Department(DepartmentId id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }

    public static Result<Department, ErrorList> Create(DepartmentId id, string name, string description)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(name))
            errors.Add(DepartmentsErrors.InvalidNameEmpty(nameof(name)));

        if (string.IsNullOrWhiteSpace(description))
            errors.Add(DepartmentsErrors.InvalidDescriptionEmpty(nameof(description)));

        if (name.Length > Constants.MAX_LOW_TEXT_LENGTH_50)
            errors.Add(DomainErrors.ValueIsLengthInvalid(nameof(name), Constants.MAX_LOW_TEXT_LENGTH_50, name.Length));
        
        if (description.Length > Constants.MAX_HIGH_TEXT_LENGTH_2000)
            errors.Add(DomainErrors.ValueIsLengthInvalid(nameof(description), Constants.MAX_LOW_TEXT_LENGTH_50, name.Length));

        if (errors.Count > 0)
            return new ErrorList(errors);

        return new Department(id, name, description);
    }
}