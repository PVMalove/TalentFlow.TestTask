using Asp.Versioning;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.API.Controllers.Department.Request;
using TalentFlow.API.Extensions;
using TalentFlow.Domain.DTO.Department;
using TalentFlow.Domain.Shared;

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

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment(
        [FromBody] CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result);
    }
}