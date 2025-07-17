using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Abstractions.Repositories;
using TalentFlow.Application.Specifications;
using TalentFlow.Domain.DTO.Department;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Infrastructure.Repositories;

public class DepartmentRepository(ApplicationDbContext context) : IDepartmentRepository
{
    public async Task<Guid> Add(Department department, CancellationToken cancellationToken = default)
    {
        await context.Departments.AddAsync(department, cancellationToken);
        return department.Id;
    }

    public Guid Save(Department department)
    {
        context.Departments.Attach(department);
        return department.Id;
    }

    public Guid Delete(Department department)
    {
        context.Departments.Remove(department);
        return department.Id;
    }

    public async Task<Result<Department, Error>> GetById(DepartmentId departmentId,
        CancellationToken cancellationToken = default)
    {
        var department = await context.Departments
            .FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);

        if (department is null)
            return Errors.General.NotFound(departmentId);

        return department;
    }

    public IQueryable<Department> GetAll()
    {
        return context.Departments.AsNoTracking().AsQueryable();
    }

    public async Task<Result<IReadOnlyList<DepartmentGetDto>, Error>> GetAllSorted(string? sortBy,
        string? sortDirection, CancellationToken cancellationToken)
    {
        var query = context.Departments?.AsNoTracking();

        if (query is null)
            return Errors.General.NotFound();

        var keySelector = SortByProperty(sortBy);

        query = sortDirection?.ToLower() == "desc"
            ? query.OrderByDescending(keySelector)
            : query.OrderBy(keySelector);

        var result = await query.Select(d => new DepartmentGetDto(d.Name, d.Description))
            .ToListAsync(cancellationToken);

        return result;
    }

    private static Expression<Func<Department, object>> SortByProperty(string? sortBy)
    {
        if (string.IsNullOrEmpty(sortBy))
            return forum => forum.Id;

        Expression<Func<Department, object>> keySelector = sortBy?.ToLower() switch
        {
            "name" => department => department.Name,
            "description" => department => department.Description
            //_ => department => department.Id
        };
        return keySelector;
    }
    
    public async Task<Department?> SingleOrDefaultWithSpecificationAsync(
        ISpecification<Department> specification,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Department> query = context.Set<Department>().ApplySpecification(specification);
        return await query.SingleOrDefaultAsync(cancellationToken);
        
        // IQueryable<Department> query = context.Set<Department>();
        //
        // if (specification.AsNoTracking)
        // {
        //     query = query.AsNoTracking();
        // }
        //
        // query = specification.Criteria.Aggregate(query, (current, condition) => 
        //     current.Where(condition));
        //
        // query = specification.Includes.Aggregate(query, (current, include) =>
        //     current.Include(include));
        //
        // return await query.SingleOrDefaultAsync(cancellationToken);
    }
}