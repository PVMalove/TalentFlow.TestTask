using TalentFlow.Domain.Shared;

namespace TalentFlow.Domain.DomainError.Departments;

public static class DepartmentsErrors
{
    public static Error InvalidNameEmpty(string name) =>
        Error.Validation(
            "Departments.InvalidNameEmpty",
            $"Название отдела не может быть пустым: '{name}'", 
            nameof(name));
    
    public static Error InvalidDescriptionEmpty(string description) =>
        Error.Validation(
            "Departments.InvalidDescriptionEmpty",
            $"Описание отдела не может быть пустым: '{description}'", 
            nameof(description));
}