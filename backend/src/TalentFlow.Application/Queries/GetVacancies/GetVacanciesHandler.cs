using System.Data.Common;
using CSharpFunctionalExtensions;
using Dapper;
using TalentFlow.Application.Abstractions;
using TalentFlow.Domain.Abstractions.Repositories;
using TalentFlow.Domain.Abstractions.Specifications;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects.EntityIds;

namespace TalentFlow.Application.Queries.GetVacancies;

public class GetVacanciesHandler(IDbConnectionFactory dbConnectionFactory, IDefaultRepository<Vacancy> vacancyRepository, ISpecificationBuilder<Vacancy> specificationBuilder) : IQueryHandlerWithResult<IReadOnlyCollection<VacancyResponse>, GetVacanciesQuery>
{
    public async Task<Result<IReadOnlyCollection<VacancyResponse>, ErrorList>> Handle(GetVacanciesQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        
        const string sql =
            $"""
             SELECT
                 id AS {nameof(VacancyResponse.Id)},
                 title AS {nameof(VacancyResponse.Title)},
                 description AS {nameof(VacancyResponse.Description)}
             FROM dbo.vacancies
             """;

        var vacancies = (await connection.QueryAsync<VacancyResponse>(sql, request)).AsList();

        var specification = specificationBuilder.AsNoTracking().Build();
        var vacancy = await vacancyRepository.FirstOrDefaultWithSpecificationAsync(specification, cancellationToken);
        int countAsync = await vacancyRepository.CountAsync(x => x.Id != VacancyId.NewId() ,cancellationToken);

        return vacancies;
    }
}
