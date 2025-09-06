using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using TalentFlow.Application.Abstractions;
using TalentFlow.Domain.Abstractions.Repositories;
using TalentFlow.Domain.DTO.Department;
using TalentFlow.Domain.Shared;


namespace TalentFlow.Application.Queries.GetAllDepartments;

public class GetAllDepartmentsHandler(IDepartmentRepository departmentRepository, ILogger<GetAllDepartmentsHandler> logger)
    : IQueryHandlerWithResult<IReadOnlyList<DepartmentGetDto>, GetAllSortedDepartmentsQuery>
{
    public async Task<Result<IReadOnlyList<DepartmentGetDto>, ErrorList>> Handle(GetAllSortedDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling GetAllDepartmentsQuery with SortBy: {SortBy}, SortDirection: {SortDirection}",
            request.SortBy, request.SortDirection);
        
        var result = await departmentRepository.GetAllSorted(request.SortBy, request.SortDirection, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToErrorList();

        return result.Value.ToList().AsReadOnly();
    }
}