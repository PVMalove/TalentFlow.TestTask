using TalentFlow.Application.Abstractions;

namespace TalentFlow.Application.Queries.GetVacancies;

public sealed record GetVacanciesQuery : IQuery<IReadOnlyCollection<VacancyResponse>>;