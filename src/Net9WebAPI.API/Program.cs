using Microsoft.OpenApi.Models;
using Net9WebAPI.API.Extensions;
using Net9WebAPI.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();
builder.ConfigurePostgreSql();
builder.ConfigureFluentValidation();
builder.ConfigureOpentelemetry();
builder.ConfigureAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi();
builder.ConfigureSwagger();

builder.RegisterPipelines();
builder.RegisterApiRequestHandlers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:8080")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.MigrateAsync();
}

app.MapPrometheusScrapingEndpoint();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi/{documentName}/openapi.json");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<HttpStatusCodeLoggingMiddleware>();
app.UseHttpLogging();
// app.UseHttpsRedirection();
app.UseCors();

app.Run();

// Mark the Program class as a partial class to support functional tests.
public partial class Program { }