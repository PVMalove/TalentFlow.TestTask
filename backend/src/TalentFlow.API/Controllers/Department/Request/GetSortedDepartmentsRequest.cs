using TalentFlow.Application.Queries.GetAllDepartments;

namespace TalentFlow.API.Controllers.Department.Request;

public record GetSortedDepartmentsRequest(
    string? SortBy,
    string? SortDirection)
{
    public GetAllSortedDepartmentsQuery ToQuery() =>
        new(
            SortBy,
            SortDirection);
}