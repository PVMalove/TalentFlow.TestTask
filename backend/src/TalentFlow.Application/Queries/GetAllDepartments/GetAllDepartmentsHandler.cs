using CSharpFunctionalExtensions;
using TalentFlow.Application.Abstractions;
using TalentFlow.Application.Abstractions.Repositories;
using TalentFlow.Domain.DTO.Department;
using TalentFlow.Domain.Shared;


namespace TalentFlow.Application.Queries.GetAllDepartments;

public class GetAllDepartmentsHandler(IDepartmentRepository departmentRepository)
    : IQueryHandlerWithResult<IReadOnlyList<DepartmentGetDto>, GetAllSortedDepartmentsQuery>
{
    public async Task<Result<IReadOnlyList<DepartmentGetDto>, ErrorList>> Handle(GetAllSortedDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await departmentRepository.GetAllSorted(request.SortBy, request.SortDirection, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToErrorList();

        return result.Value.ToList().AsReadOnly();
    }
}