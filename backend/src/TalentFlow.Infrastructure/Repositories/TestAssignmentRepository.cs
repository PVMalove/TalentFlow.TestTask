using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Abstractions.Repositories;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Infrastructure.Repositories;

public class TestAssignmentRepository (ApplicationDbContext context) : ITestAssignmentRepository
{
    public async Task<Guid> Add(TestAssignment testAssignment, CancellationToken cancellationToken = default)
    {
        await context.TestAssignments.AddAsync(testAssignment, cancellationToken);
        return testAssignment.Id;
    }

    public Guid Save(TestAssignment testAssignment)
    {
        context.TestAssignments.Attach(testAssignment);
        return testAssignment.Id;
    }

    public Guid Delete(TestAssignment testAssignment)
    {
        context.TestAssignments.Remove(testAssignment);
        return testAssignment.Id;
    }

    public async Task<Result<TestAssignment, Error>> GetById(TestAssignmentId testAssignmentId,
        CancellationToken cancellationToken = default)
    {
        var testAssignment = await context.TestAssignments
            .FirstOrDefaultAsync(ta => ta.Id == testAssignmentId, cancellationToken);

        if (testAssignment is null)
            return Errors.General.NotFound(testAssignmentId);

        return testAssignment;
    }
}