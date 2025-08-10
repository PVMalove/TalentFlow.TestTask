using CSharpFunctionalExtensions;
using TalentFlow.Application.Abstractions;
using TalentFlow.Application.Abstractions.Common;
using TalentFlow.Domain.Abstractions.Repositories;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects.EntityIds;

namespace TalentFlow.Application.Commands.CreateDepartment;

public class CreateDepartmentHandler(IDepartmentRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<Guid, CreateDepartmentCommand>
{
    public async Task<Result<Guid, ErrorList>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var departmentId = DepartmentId.NewId();
        var department = Department.Create(departmentId, request.Name, request.Description);
        if (department.IsFailure)
            return department.Error.ToErrorList();
        
        var result = await repository.Add(department.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}