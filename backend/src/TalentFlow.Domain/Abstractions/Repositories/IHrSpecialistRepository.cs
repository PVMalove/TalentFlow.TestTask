using CSharpFunctionalExtensions;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.Shared;
using TalentFlow.Domain.ValueObjects.EntityIds;

namespace TalentFlow.Domain.Abstractions.Repositories;

public interface IHrSpecialistRepository
{
    Task<Guid> Add(HrSpecialist hr, CancellationToken cancellationToken = default);
    
    Guid Save(HrSpecialist hr);
    
    Guid Delete(HrSpecialist hr);

    Task<Result<HrSpecialist, Error>> GetById(HrSpecialistId hrId,
        CancellationToken cancellationToken = default);
}