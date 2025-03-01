using CSharpFunctionalExtensions;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Application.Abstractions.Repositories;

public interface ITestAssignmentRepository
{
    Task<Guid> Add(TestAssignment testAssignment, CancellationToken cancellationToken = default);
    
    Guid Save(TestAssignment testAssignment);
    
    Guid Delete(TestAssignment testAssignment);

    Task<Result<TestAssignment, Error>> GetById(TestAssignmentId testAssignmentId,
        CancellationToken cancellationToken = default);
}