using CSharpFunctionalExtensions;
using TalentFlow.Domain.DTO.Department;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Application.Abstractions.Repositories;

public interface IDepartmentRepository
{
    public Task<Guid> Add(Department department, CancellationToken cancellationToken = default);

    public Guid Save(Department department);

    public Guid Delete(Department department);

    public Task<Result<Department, Error>> GetById(DepartmentId departmentId,
        CancellationToken cancellationToken = default);

    public Task<Result<IReadOnlyList<DepartmentGetDto>, Error>> GetAllSorted(string? sortBy,
        string? sortDirection, CancellationToken cancellationToken);
}