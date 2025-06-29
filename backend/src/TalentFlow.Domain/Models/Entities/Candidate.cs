using CSharpFunctionalExtensions;
using TalentFlow.Domain.DomainError;
using TalentFlow.Domain.Models.ValueObjects;
using TalentFlow.Domain.Models.ValueObjects.EntityIds;
using TalentFlow.Domain.Shared;

namespace TalentFlow.Domain.Models.Entities;

public class Candidate : Shared.Entity<CandidateId>
{
    public FullName FullName { get; init; } = null!;
    public ContactInfo ContactInfo { get; init; } = null!;
    public string ResumeUrl { get; init; } = null!;
    
    protected Candidate(CandidateId id) : base(id){}

    private Candidate(CandidateId id, FullName fullName, ContactInfo contactInfo, string resumeUrl) : base(id)
    {
        FullName = fullName;
        ContactInfo = contactInfo;
        ResumeUrl = resumeUrl;
    }
    
    public static Result<Candidate, Error> Create(CandidateId id, FullName fullName, ContactInfo contactInfo, string resumeUrl)
    {
        if (string.IsNullOrWhiteSpace(fullName.FirstName))
            return DomainErrors.ValueIsInvalid("fullName.FirstName");
        
        if (string.IsNullOrWhiteSpace(fullName.SecondName))
            return DomainErrors.ValueIsInvalid("fullName.SecondName");
        
        return new Candidate(id, fullName, contactInfo, resumeUrl);
    }
}