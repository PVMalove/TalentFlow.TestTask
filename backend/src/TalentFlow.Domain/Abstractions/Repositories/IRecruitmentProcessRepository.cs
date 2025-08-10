using CSharpFunctionalExtensions;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects.EntityIds;

namespace TalentFlow.Domain.Abstractions.Repositories;

public interface IRecruitmentProcessRepository
{
    Task<Guid> Add(RecruitmentProcess recruitmentProcess, CancellationToken cancellationToken = default);
    
    Guid Save(RecruitmentProcess recruitmentProcess);
    
    Guid Delete(RecruitmentProcess recruitmentProcess);

    Task<Result<RecruitmentProcess, Error>> GetById(RecruitmentProcessId recruitmentProcessId,
        CancellationToken cancellationToken = default);
}