using Net9WebAPI.WebAPI.Extensions;
using Net9WebAPI.WebAPI.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();
builder.ConfigurePostgreSql();
builder.ConfigureFluentValidation();
builder.ConfigureOpentelemetry();
builder.ConfigureAuthentication();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.RegisterApiRepositories();
builder.RegisterPipelines();
builder.RegisterApiRequestHandlers();

var app = builder.Build();

await app.MigrateAsync();

app.MapPrometheusScrapingEndpoint();
app.UseAuthentication();

app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi/{documentName}/openapi.json");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<HttpStatusCodeLoggingMiddleware>();
app.UseHttpLogging();
app.UseHttpsRedirection();

app.Run();

// Mark the Program class as a partial class to support functional tests.
public partial class Program { }