using System.Data.Common;
using CSharpFunctionalExtensions;
using Dapper;
using TalentFlow.Application.Abstractions;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Application.Queries.GetVacancies;

public class GetVacanciesHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandlerWithResult<IReadOnlyCollection<VacancyResponse>, GetVacanciesQuery>
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

        return vacancies;
    }
}
