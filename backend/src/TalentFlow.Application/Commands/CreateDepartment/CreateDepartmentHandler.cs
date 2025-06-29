using CSharpFunctionalExtensions;
using TalentFlow.Application.Abstractions;
using TalentFlow.Application.Abstractions.Common;
using TalentFlow.Application.Abstractions.Repositories;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Application.Commands.CreateDepartment;

public class CreateDepartmentHandler(IDepartmentRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<Guid, CreateDepartmentCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var departmentId = DepartmentId.NewId();
        var department = Department.Create(departmentId, request.Name, request.Description);
        if (department.IsFailure)
            return department.Error;
        
        var result = await repository.Add(department.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}