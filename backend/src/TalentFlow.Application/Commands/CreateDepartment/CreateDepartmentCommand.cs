using TalentFlow.Application.Abstractions;

namespace TalentFlow.Application.Commands.CreateDepartment;

public record CreateDepartmentCommand(string Name, string Description) : ICommand<Guid>;