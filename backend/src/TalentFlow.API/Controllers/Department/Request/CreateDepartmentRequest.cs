using TalentFlow.Application.Commands.CreateDepartment;

namespace TalentFlow.API.Controllers.Department.Request;

public record CreateDepartmentRequest(
    string name,
    string description)
{
    public CreateDepartmentCommand ToCommand() =>
        new(name, description
        );
}