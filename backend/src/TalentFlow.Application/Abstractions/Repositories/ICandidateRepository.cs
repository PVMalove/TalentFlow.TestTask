using CSharpFunctionalExtensions;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Application.Abstractions.Repositories;

public interface ICandidateRepository
{
    Task<Guid> Add(Candidate candidate, CancellationToken cancellationToken = default);

    Guid Save(Candidate candidate);

    Guid Delete(Candidate candidate);

    Task<Result<Candidate, Error>> GetById(CandidateId candidateId,
        CancellationToken cancellationToken = default);
}