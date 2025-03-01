using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.API.Controllers.Department.Request;
using TalentFlow.API.Extensions;

namespace TalentFlow.API.Controllers.Department;

[ApiVersion("1.0")]
public class DepartmentsController(IMediator mediator) : ApplicationController
{
    [HttpGet]
    public async Task<IActionResult> GetDepartments(
        [FromQuery] GetSortedDepartmentsRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        var result = await mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}