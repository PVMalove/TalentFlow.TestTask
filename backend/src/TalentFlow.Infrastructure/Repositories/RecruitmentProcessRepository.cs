using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Abstractions.Repositories;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Infrastructure.Repositories;

public class RecruitmentProcessRepository(ApplicationDbContext context) : IRecruitmentProcessRepository
{
    public async Task<Guid> Add(RecruitmentProcess recruitmentProcess, CancellationToken cancellationToken = default)
    {
        await context.RecruitmentProcesses.AddAsync(recruitmentProcess, cancellationToken);
        return recruitmentProcess.Id;
    }

    public Guid Save(RecruitmentProcess recruitmentProcess)
    {
        context.RecruitmentProcesses.Attach(recruitmentProcess);
        return recruitmentProcess.Id;
    }

    public Guid Delete(RecruitmentProcess recruitmentProcess)
    {
        context.RecruitmentProcesses.Remove(recruitmentProcess);
        return recruitmentProcess.Id;
    }

    public async Task<Result<RecruitmentProcess, Error>> GetById(RecruitmentProcessId recruitmentProcessId,
        CancellationToken cancellationToken = default)
    {
        var recruitmentProcess = await context.RecruitmentProcesses
            .FirstOrDefaultAsync(rp => rp.Id == recruitmentProcessId, cancellationToken);

        if (recruitmentProcess is null)
            return Errors.General.NotFound(recruitmentProcessId);

        return recruitmentProcess;
    }
}