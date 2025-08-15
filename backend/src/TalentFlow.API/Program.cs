using Asp.Versioning.ApiExplorer;
using TalentFlow.API;
using TalentFlow.API.Extensions;
using TalentFlow.API.Extensions.Endpoints;
using TalentFlow.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddCustomServices(builder);
services.AddEndpoints(AssemblyReference.Assembly);

var app = builder.Build();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

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
app.MapControllers();
app.MapEndpoints();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.Run();

public partial class Program;