using TalentFlow.Application.Abstractions;
using TalentFlow.Domain.Models.Entities;

namespace TalentFlow.Application.Commands.CreateDepartment;

public record CreateDepartmentCommand(string Name, string Description) : ICommand<Guid>;