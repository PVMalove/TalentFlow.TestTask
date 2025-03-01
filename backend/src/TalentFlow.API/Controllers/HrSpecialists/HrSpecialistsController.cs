using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.API.Controllers.HrSpecialists.Request;
using TalentFlow.API.Extensions;

namespace TalentFlow.API.Controllers.HrSpecialists;

[ApiVersion("1.0")]
public class HrSpecialistsController(IMediator mediator) : ApplicationController
{
    [HttpDelete("{Id}/remove-vacancy")]
    public async Task<IActionResult> RemoveVacancy(
        Guid id,
        [FromBody] RemoveVacancyWithHrSpecialistRequest request)
    {
        var command = request.ToCommand(id);
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
}