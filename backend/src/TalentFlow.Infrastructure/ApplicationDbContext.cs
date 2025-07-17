using Microsoft.EntityFrameworkCore;
using TalentFlow.Application.Abstractions.Repositories;
using TalentFlow.Domain.Models.Entities;

namespace TalentFlow.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<HrSpecialist> HRSpecialists { get; set; }
    public DbSet<Vacancy> Vacancies { get; set; }
    public DbSet<Candidate> Candidates { get; set; }
    public DbSet<RecruitmentProcess> RecruitmentProcesses { get; set; }
    public DbSet<TestAssignment> TestAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}