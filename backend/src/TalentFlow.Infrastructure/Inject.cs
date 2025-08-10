using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TalentFlow.Application.Abstractions;
using TalentFlow.Application.Abstractions.Common;
using TalentFlow.Domain.Abstractions.Repositories;
using TalentFlow.Domain.Abstractions.Specifications;
using TalentFlow.Infrastructure.Common;
using TalentFlow.Infrastructure.Repositories;
using TalentFlow.Infrastructure.Specifications;

namespace TalentFlow.Infrastructure;

public static class Inject
{
    private const string DATABASE = "DbConnection";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(DATABASE);
        services.AddDbContextPool<ApplicationDbContext>(options =>
            options
                .UseSqlServer(connectionString)
                .UseSnakeCaseNamingConvention());
        
        services.AddScoped<IDbConnectionFactory>(_ =>
            new DbConnectionFactory(connectionString));
 
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<DbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped(typeof(IDefaultRepository<>), typeof(DefaultRepository<>));
        services.AddScoped(typeof(ISpecificationBuilder<>), typeof(SpecificationBuilder<>));
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<IRecruitmentProcessRepository, RecruitmentProcessRepository>();
        services.AddScoped<IVacancyRepository, VacancyRepository>();
        services.AddScoped<ITestAssignmentRepository, TestAssignmentRepository>();
        services.AddScoped<IHrSpecialistRepository, HrSpecialistRepository>();

        return services;
    }
}