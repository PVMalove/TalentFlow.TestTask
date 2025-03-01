using TalentFlow.Application.Abstractions;
using TalentFlow.Domain.DTO;
using TalentFlow.Domain.DTO.RecruitmentProcess;
using TalentFlow.Domain.DTO.TestAssignment;


namespace TalentFlow.Application.Commands.CreateRecruitmentProcess;

public record CreateRecruitmentProcessCommand(
    Guid VacancyId,
    Guid CandidateId,
    TestAssignmentDto? TestAssignment) : ICommand<RecruitmentProcessDto>;