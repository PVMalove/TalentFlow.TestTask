using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TalentFlow.Domain.Abstractions.Repositories;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects.EntityIds;

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