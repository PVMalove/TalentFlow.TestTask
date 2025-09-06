using System.Diagnostics;
using Asp.Versioning.ApiExplorer;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using TalentFlow.API;
using TalentFlow.API.Extensions;
using TalentFlow.API.Extensions.Endpoints;
using TalentFlow.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddCustomServices(builder);
services.AddEndpoints(AssemblyReference.Assembly);
services.AddApiLogging(builder.Configuration);

// Добавляем OpenTelemetry
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(
            serviceName: builder.Configuration["ServiceName"] ?? "TalentFlow.API",
            serviceVersion: "1.0.0",
            serviceInstanceId: Environment.MachineName))
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddMeter("TalentFlow.API.Exceptions")
            .AddPrometheusExporter(options =>
            {
                options.ScrapeEndpointPath = "/metrics";
                options.DisableTotalNameSuffixForCounters = false;
            });
    })
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = true;
                options.EnrichWithException = (activity, exception) =>
                {
                    activity.SetTag("exception.type", exception.GetType().Name);
                    activity.SetTag("exception.message", exception.Message);
                    activity.SetStatus(ActivityStatusCode.Error);
                };
            })
            .AddHttpClientInstrumentation(options =>
            {
                options.RecordException = true;
            })
            .AddSource("TalentFlow.API");
    });

WebApplication app = builder.Build();
IApiVersionDescriptionProvider apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    await app.DbInitializer();
    
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
        {
            config.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                $"Forum swagger {description.GroupName.ToUpperInvariant()}");
        }
    });
}

app.UseCors(x=> x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapPrometheusScrapingEndpoint();
app.MapControllers();
app.MapEndpoints();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.Run();

public partial class Program;