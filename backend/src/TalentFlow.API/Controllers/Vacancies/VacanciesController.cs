using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.API.Extensions;
using TalentFlow.Application.Queries.GetVacancies;

namespace TalentFlow.API.Controllers.Vacancies;

[ApiVersion("1.0")]
public class VacanciesController(IMediator mediator) : ApplicationController
{
    [HttpGet]
    public async Task<IActionResult> GetVacancies(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetVacanciesQuery(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}