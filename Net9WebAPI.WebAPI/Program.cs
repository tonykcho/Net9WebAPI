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

var app = builder.Build();

await app.MigrateAsync();

app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi/{documentName}/openapi.json");
}

// Configure dotnet Middleware
app.UseHttpLogging();
app.UseHttpsRedirection();

app.Run();

// Mark the Program class as a partial class to support functional tests.
public partial class Program { }