using CSharpFunctionalExtensions;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects.EntityIds;

namespace TalentFlow.Domain.Abstractions.Repositories;

public interface ITestAssignmentRepository
{
    Task<Guid> Add(TestAssignment testAssignment, CancellationToken cancellationToken = default);
    
    Guid Save(TestAssignment testAssignment);
    
    Guid Delete(TestAssignment testAssignment);

    Task<Result<TestAssignment, Error>> GetById(TestAssignmentId testAssignmentId,
        CancellationToken cancellationToken = default);
}