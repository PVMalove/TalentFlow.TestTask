using TalentFlow.Application.Abstractions;
using TalentFlow.Domain.DTO.Department;


namespace TalentFlow.Application.Queries.GetAllDepartments;

public record GetAllSortedDepartmentsQuery(
    string? SortBy,
    string? SortDirection): IQuery<IReadOnlyList<DepartmentGetDto>>;