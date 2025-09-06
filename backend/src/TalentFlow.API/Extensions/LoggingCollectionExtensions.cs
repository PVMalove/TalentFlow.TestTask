using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

namespace TalentFlow.API.Extensions;

internal static class LoggingCollectionExtensions
{
    public static IServiceCollection AddApiLogging(this IServiceCollection services, IConfiguration configuration)
    {
        LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .MinimumLevel.Override("System", LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("application", "TalentFlow.API")
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName();

        var lokiUrl = configuration.GetConnectionString("LokiLogsUrl");
        
        if (!string.IsNullOrEmpty(lokiUrl))
        {
            loggerConfiguration.WriteTo.GrafanaLoki(
                lokiUrl,
                labels: [new LokiLabel { Key = "job", Value = "TalentFlow" }],
                propertiesAsLabels: ["application", "level"],
                restrictedToMinimumLevel: LogEventLevel.Information
            );
        }

        Log.Logger = loggerConfiguration.CreateLogger();

        return services.AddLogging(b => b.AddSerilog(Log.Logger));
    }
}