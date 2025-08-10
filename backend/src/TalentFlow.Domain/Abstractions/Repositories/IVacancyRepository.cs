using CSharpFunctionalExtensions;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects.EntityIds;

namespace TalentFlow.Domain.Abstractions.Repositories;

public interface IVacancyRepository
{
    Task<Guid> Add(Vacancy vacancy, CancellationToken cancellationToken = default);
    
    Guid Save(Vacancy vacancy);
    
    Guid Delete(Vacancy vacancy);

    Task<Result<Vacancy, Error>> GetById(VacancyId vacancyId,
        CancellationToken cancellationToken = default);
}