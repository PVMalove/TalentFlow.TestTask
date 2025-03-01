﻿using TalentFlow.Application.Commands.CreateRecruitmentProcess;
using TalentFlow.Domain.DTO;
using TalentFlow.Domain.DTO.TestAssignment;
using TalentFlow.Domain.Models.Entities;

namespace TalentFlow.API.Controllers.RecruitmentProcess.Request;

public record CreateRecruitmentProcessRequest(
    Guid VacancyId,
    Guid CandidateId,
    TestAssignmentDto? TestAssigment = null)
{
    public CreateRecruitmentProcessCommand ToCommand() =>
        new(VacancyId, CandidateId, TestAssigment);
}