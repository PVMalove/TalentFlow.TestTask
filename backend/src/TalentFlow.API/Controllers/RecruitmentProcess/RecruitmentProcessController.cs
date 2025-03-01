using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.API.Controllers.RecruitmentProcess.Request;
using TalentFlow.API.Extensions;

namespace TalentFlow.API.Controllers.RecruitmentProcess;

[ApiVersion("1.0")]
public class RecruitmentProcessController(IMediator mediator) : ApplicationController
{
    [HttpPost]
    [ProducesResponseType(400)]
    [ProducesResponseType(410)]
    [ProducesResponseType(200)]
    public async Task<ActionResult> CreateRecruitmentProcess(
        [FromBody] CreateRecruitmentProcessRequest request,
        CancellationToken cancellationToken)
    {
        var query = request.ToCommand();
        var result = await mediator.Send(query, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}