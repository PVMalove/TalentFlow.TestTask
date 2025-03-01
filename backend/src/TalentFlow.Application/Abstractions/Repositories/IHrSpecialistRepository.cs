using CSharpFunctionalExtensions;
using TalentFlow.Domain.Models.Entities;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Application.Abstractions.Repositories;

public interface IHrSpecialistRepository
{
    Task<Guid> Add(HrSpecialist hr, CancellationToken cancellationToken = default);
    
    Guid Save(HrSpecialist hr);
    
    Guid Delete(HrSpecialist hr);

    Task<Result<HrSpecialist, Error>> GetById(HrSpecialistId hrId,
        CancellationToken cancellationToken = default);
}