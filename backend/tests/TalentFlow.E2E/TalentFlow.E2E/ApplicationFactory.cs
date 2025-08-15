using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TalentFlow.Domain.Entities;
using TalentFlow.Domain.ValueObjects.EntityIds;
using TalentFlow.Infrastructure;
using Testcontainers.MsSql;

namespace TalentFlow.E2E;

public class ApplicationFactory(MsSqlContainer dbContainer) : WebApplicationFactory<Program>
{
    private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
    {
        builder
            .AddConsole()
            .SetMinimumLevel(LogLevel.Warning)
            .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning)
            .AddFilter("Testcontainers", LogLevel.Information);
    });

    private readonly ILogger<ApplicationFactory> _logger =
        _loggerFactory.CreateLogger<ApplicationFactory>();

    private readonly string _connectionString = dbContainer.GetConnectionString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole()
                .SetMinimumLevel(LogLevel.Information);
        });

        builder.ConfigureServices(services =>
        {
            RemoveDbContext<ApplicationDbContext>(services);

            services.AddDbContextFactory<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_connectionString)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });

            services.AddScoped<ApplicationDbContext, TestContext>();
        });
    }

    private static void RemoveDbContext<TContext>(IServiceCollection services) where TContext : DbContext
    {
        var contextType = typeof(TContext);
        var descriptors = services
            .Where(s => s.ServiceType == typeof(DbContextOptions<TContext>) ||
                        s.ServiceType == typeof(DbContextOptions) ||
                        s.ServiceType == contextType ||
                        s.ServiceType == typeof(IDbContextPool<TContext>) ||
                        s.ServiceType == typeof(IScopedDbContextLease<TContext>))
            .ToList();

        foreach (var descriptor in descriptors)
        {
            services.Remove(descriptor);
        }
    }

    public async Task InitializeDatabaseAsync()
    {
        await using var scope = Services.CreateAsyncScope();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();
        await SeedDataAsync(context);
    }

    private async Task SeedDataAsync(ApplicationDbContext context)
    {
        var departmentId = DepartmentId.NewId();
        var department = Department.Create(departmentId, "Department Name 1", "Department Description 1");
        context.Departments.Add(department.Value);
        await context.SaveChangesAsync();
    }
}