using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Abstractions.Repositories;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Infrastructure.Repositories;

public class HrSpecialistRepository(ApplicationDbContext context) : IHrSpecialistRepository
{
    public async Task<Guid> Add(HrSpecialist hr, CancellationToken cancellationToken = default)
    {
        await context.HRSpecialists.AddAsync(hr, cancellationToken);
        return hr.Id;
    }

    public Guid Save(HrSpecialist hr)
    {
        context.HRSpecialists.Attach(hr);
        return hr.Id;
    }

    public Guid Delete(HrSpecialist hr)
    {
        context.HRSpecialists.Remove(hr);
        return hr.Id;
    }

    public async Task<Result<HrSpecialist, Error>> GetById(HrSpecialistId hrId,
        CancellationToken cancellationToken = default)
    {
        var hr = await context.HRSpecialists
            .FirstOrDefaultAsync(c => c.Id == hrId, cancellationToken);

        if (hr is null)
            return Errors.General.NotFound(hrId);

        return hr;
    }
}