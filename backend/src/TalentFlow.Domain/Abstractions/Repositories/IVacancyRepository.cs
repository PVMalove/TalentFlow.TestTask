using CSharpFunctionalExtensions;
using TalentFlow.Application.Specifications;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Application.Abstractions.Repositories;

public interface IVacancyRepository
{
    Task<Guid> Add(Vacancy vacancy, CancellationToken cancellationToken = default);
    
    Guid Save(Vacancy vacancy);
    
    Guid Delete(Vacancy vacancy);

    Task<Result<Vacancy, Error>> GetById(VacancyId vacancyId,
        CancellationToken cancellationToken = default);

    Task<Vacancy?> SingleOrDefaultWithSpecificationAsync(
        ISpecification<Vacancy> specification,
        CancellationToken cancellationToken = default);
}