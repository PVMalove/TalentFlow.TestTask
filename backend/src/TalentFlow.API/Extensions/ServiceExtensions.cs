using Asp.Versioning;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using TalentFlow.API.Configurations;
using TalentFlow.Application;
using TalentFlow.Infrastructure;

namespace TalentFlow.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddControllers();

        services.AddInfrastructure(builder.Configuration);
        services.AddApplication();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        
         services.AddApiVersioning()
             .AddApiExplorer(options =>
             {
                 options.DefaultApiVersion = new ApiVersion(1.0);
                 options.GroupNameFormat = "'v'VVV";
                 options.SubstituteApiVersionInUrl = true;
                 options.AssumeDefaultVersionWhenUnspecified = true;
             });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
}