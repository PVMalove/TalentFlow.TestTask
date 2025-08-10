using CSharpFunctionalExtensions;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects.EntityIds;

namespace TalentFlow.Domain.Abstractions.Repositories;

public interface ICandidateRepository
{
    Task<Guid> Add(Candidate candidate, CancellationToken cancellationToken = default);

    Guid Save(Candidate candidate);

    Guid Delete(Candidate candidate);

    Task<Result<Candidate, Error>> GetById(CandidateId candidateId,
        CancellationToken cancellationToken = default);
}