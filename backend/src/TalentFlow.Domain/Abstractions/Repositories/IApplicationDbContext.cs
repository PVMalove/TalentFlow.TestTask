using Microsoft.EntityFrameworkCore;
using TalentFlow.Domain.Models.Entities;

namespace TalentFlow.Application.Abstractions.Repositories;

public interface IApplicationDbContext
{
    DbSet<Department> Departments { get; set; }
    DbSet<HrSpecialist> HRSpecialists { get; set; }
    DbSet<Vacancy> Vacancies { get; set; }
    DbSet<Candidate> Candidates { get; set; }
    DbSet<RecruitmentProcess> RecruitmentProcesses { get; set; }
    DbSet<TestAssignment> TestAssignments { get; set; }
}