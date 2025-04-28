using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Net9WebAPI.Application.Abstract;
using Net9WebAPI.DataAccess.DbContexts;
using Net9WebAPI.DataAccess.Repositories;
using Net9WebAPI.WebAPI.Pipelines;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using Microsoft.AspNetCore.HttpLogging;
using Net9WebAPI.Domain.Abstract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

namespace Net9WebAPI.WebAPI.Extensions;


public static class WebApplicationBuilderExtension
{
    const string ApplicationAssemblyName = "Net9WebAPI.Application";
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        var labels = new List<LokiLabel>
        {
            new LokiLabel { Key = "app", Value = builder.Environment.ApplicationName },
            new LokiLabel { Key = "env", Value = builder.Environment.EnvironmentName }
        };

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.GrafanaLoki(builder.Configuration["Loki:Endpoint"]!, labels)
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
            logging.CombineLogs = true;
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

    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        string secretKey = builder.Configuration["JwtBearerSecretKey"] ?? "default_secret_key";
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://localhost:8001",
                    ValidateAudience = true,
                    ValidAudience = "Net9WebApi",
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
    }

    public static void ConfigureSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT Token with bearer format"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
    }
}