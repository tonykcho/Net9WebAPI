using Net9WebAPI.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();
builder.ConfigurePostgreSql();
builder.ConfigureFluentValidation();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi();

builder.RegisterApiRepositories();
builder.RegisterPipelines();
builder.RegisterApiRequestHandlers();

// Configure Aspire default services (HealthCheck, OpenTelemetry)
builder.AddServiceDefaults();

var app = builder.Build();

await app.MigrateAsync();

app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi/{documentName}/openapi.json");
}

// Configure Aspire default endpoints (HealthCheck)
app.MapDefaultEndpoints();

// Configure dotnet Middleware
app.UseHttpLogging();
app.UseHttpsRedirection();

app.Run();

// Mark the Program class as a partial class to support functional tests.
public partial class Program { }