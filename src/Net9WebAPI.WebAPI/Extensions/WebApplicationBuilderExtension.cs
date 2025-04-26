using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Net9WebAPI.Application.Abstract;
using Net9WebAPI.DataAccess.Abstract;
using Net9WebAPI.DataAccess.DbContexts;
using Net9WebAPI.DataAccess.Repositories;
using Net9WebAPI.WebAPI.Pipelines;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using OpenTelemetry.Metrics;

namespace Net9WebAPI.WebAPI.Extensions;


public static class WebApplicationBuilderExtension
{
    const string ApplicationAssemblyName = "Net9WebAPI.Application";
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();

        builder.Host.UseSerilog();

        builder.Services.AddHttpLogging(logging =>
        {
            logging.LoggingFields =
                HttpLoggingFields.RequestPath
                | HttpLoggingFields.RequestMethod
                | HttpLoggingFields.ResponseStatusCode
                | HttpLoggingFields.ResponseBody;
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
    }

    public static void ConfigurePostgreSql(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<Net9WebAPIDbContext>();
    }

    public static void ConfigureFluentValidation(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        builder.Services.AddValidatorsFromAssembly(Assembly.Load(ApplicationAssemblyName));
    }

    public static void ConfigureOpentelemetry(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
            .WithMetrics(options =>
            {
                options
                    .AddProcessInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddMeter("Net9WebAPIMeter")
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspN>etCore.Server.Kestrel")
                    .AddPrometheusExporter();
            })
            .WithTracing(tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddOtlpExporter(opt =>
                    {
                        var endpoint = builder.Configuration["OTLP:Endpoint"];
                        if (string.IsNullOrEmpty(endpoint))
                        {
                            throw new InvalidOperationException("OTLP:Endpoint configuration is missing or empty.");
                        }
                        opt.Endpoint = new Uri(endpoint);
                    });
            });
    }

    public static void RegisterApiRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
    }

    public static void RegisterPipelines(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ApiRequestPipeline>();
    }

    public static void RegisterApiRequestHandlers(this WebApplicationBuilder builder)
    {
        foreach (var type in Assembly.Load(ApplicationAssemblyName).GetTypes().Where(x => x.Name.EndsWith("RequestHandler") && x.IsAbstract == false && x.IsInterface == false))
        {
            foreach (var iface in type.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IApiRequestHandler<>)))
            {
                builder.Services.AddTransient(iface, type);
            }
        }
    }
}