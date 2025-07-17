using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Abstractions;
using TalentFlow.Application.Abstractions.Repositories;
using TalentFlow.Application.Specifications;
using TalentFlow.Domain.DTO.Department;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;


namespace TalentFlow.Application.Queries.GetAllDepartments;

public class GetAllDepartmentsHandler(IDepartmentRepository departmentRepository, IApplicationDbContext context)
    : IQueryHandlerWithResult<IReadOnlyList<DepartmentGetDto>, GetAllSortedDepartmentsQuery>
{
    public async Task<Result<IReadOnlyList<DepartmentGetDto>, ErrorList>> Handle(GetAllSortedDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        var vacancySpecification = Specification<Department>.Builder()
            .Where(v => v.Id == DepartmentId.Create(Guid.Parse("c52e5ca0-9f34-4760-b4c3-3679fd1a9737")))
            .Include(v => v.Vacancies.Where(vacancy => vacancy.Title == "222"))
            .Build();
        
        var result2 = await departmentRepository.SingleOrDefaultWithSpecificationAsync(vacancySpecification, cancellationToken);

        var result3 = await context.Departments
            .Include(v => v.Vacancies)
            .SingleOrDefaultAsync(v => v.Id == DepartmentId.Create(Guid.Parse("c52e5ca0-9f34-4760-b4c3-3679fd1a9737")), cancellationToken);
        
    
        
        var result = await departmentRepository.GetAllSorted(request.SortBy, request.SortDirection, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToErrorList();

        return result.Value.ToList().AsReadOnly();
    }
}