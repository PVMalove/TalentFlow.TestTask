using CSharpFunctionalExtensions;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Application.Abstractions.Repositories;

public interface IRecruitmentProcessRepository
{
    Task<Guid> Add(RecruitmentProcess recruitmentProcess, CancellationToken cancellationToken = default);
    
    Guid Save(RecruitmentProcess recruitmentProcess);
    
    Guid Delete(RecruitmentProcess recruitmentProcess);

    Task<Result<RecruitmentProcess, Error>> GetById(RecruitmentProcessId recruitmentProcessId,
        CancellationToken cancellationToken = default);
}