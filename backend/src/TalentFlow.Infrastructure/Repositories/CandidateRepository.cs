﻿using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Abstractions.Repositories;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Infrastructure.Repositories;

public class CandidateRepository(ApplicationDbContext context) : ICandidateRepository
{
    public async Task<Guid> Add(Candidate candidate, CancellationToken cancellationToken = default)
    {
        await context.Candidates.AddAsync(candidate, cancellationToken);
        return candidate.Id;
    }

    public Guid Save(Candidate candidate)
    {
        context.Candidates.Attach(candidate);
        return candidate.Id;
    }

    public Guid Delete(Candidate candidate)
    {
        context.Candidates.Remove(candidate);
        return candidate.Id;
    }

    public async Task<Result<Candidate, Error>> GetById(CandidateId candidateId,
        CancellationToken cancellationToken = default)
    {
        var candidate = await context.Candidates
            .FirstOrDefaultAsync(c => c.Id == candidateId, cancellationToken);

        if (candidate is null)
            return Errors.General.NotFound(candidateId);

        return candidate;
    }
}