using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Abstractions.Repositories;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Infrastructure.Repositories;

public class VacancyRepository(ApplicationDbContext context) : IVacancyRepository
{
    public async Task<Guid> Add(Vacancy vacancy, CancellationToken cancellationToken = default)
    {
        await context.Vacancies.AddAsync(vacancy, cancellationToken);
        return vacancy.Id;
    }

    public Guid Save(Vacancy vacancy)
    {
        context.Vacancies.Attach(vacancy);
        return vacancy.Id;
    }

    public Guid Delete(Vacancy vacancy)
    {
        context.Vacancies.Remove(vacancy);
        return vacancy.Id;
    }

    public async Task<Result<Vacancy, Error>> GetById(VacancyId vacancyId,
        CancellationToken cancellationToken = default)
    {
        var vacancy = await context.Vacancies
            .FirstOrDefaultAsync(v => v.Id == vacancyId, cancellationToken);

        if (vacancy is null)
            return Errors.General.NotFound(vacancyId);

        return vacancy;
    }
}