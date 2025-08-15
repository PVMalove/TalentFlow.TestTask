using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TalentFlow.API.Controllers.Department.Request;
using TalentFlow.API.Controllers.TestAssignment.Request;
using TalentFlow.API.Extensions.Endpoints;
using TalentFlow.API.Extensions.New;
using TalentFlow.Domain.DTO.Department;
using TalentFlow.Domain.Shared;

namespace TalentFlow.API.Endpoints;

internal sealed class EndPoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/endpoints/departments/",
            async ([FromBody] CreateDepartmentRequest request, ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = request.ToCommand();
                Result<Guid, ErrorList> result = await sender.Send(command, cancellationToken);
                return result.EndpointMatchOk();
            });
        
        app.MapGet("api/v1/endpoints/departments/",
            async ([AsParameters] GetSortedDepartmentsRequest request, ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = request.ToQuery();
                Result<IReadOnlyList<DepartmentGetDto>, ErrorList> result = await sender.Send(query, cancellationToken);
                return result.EndpointMatchOk();
            });

        app.MapPut("api/v1/endpoints/test-assignments/{id:guid}/status",
            async (Guid id, ChangeTestAssignmentStatusRequest request, ISender sender) =>
            {
                var command = request.ToCommand(id);
                UnitResult<ErrorList> result = await sender.Send(command);
                return result.EndpointMatchNoContent();
            });
    }
}