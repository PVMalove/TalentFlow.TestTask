using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.API.Controllers.TestAssignment.Request;
using TalentFlow.API.Extensions;

namespace TalentFlow.API.Controllers.TestAssignment;

[ApiVersion("1.0")]
public class TestAssignmentController(IMediator mediator) : ApplicationController
{
    [HttpPut("{id}/status")]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        [FromBody] ChangeTestAssignmentStatusRequest request)
    {
        var command = request.ToCommand(id);
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
}