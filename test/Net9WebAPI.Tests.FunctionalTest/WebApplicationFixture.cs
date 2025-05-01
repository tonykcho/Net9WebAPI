
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Net9WebAPI.DataAccess.DbContexts;
using Testcontainers.PostgreSql;

namespace Net9WebAPI.Tests.FunctionalTest;

public class WebApplicationFixture : IAsyncLifetime
{
    public PostgreSqlContainer PostgreSqlContainer { get; set; }

    public WebApplicationFixture()
    {
        PostgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("Net9WebAPI")
            .WithPortBinding(54321, 5432)
            .WithUsername("postgres")
            .WithPassword("pwd")
            .Build();
    }

    public HttpClient Client { get; set; } = null!;

    public async Task DisposeAsync()
    {
        await PostgreSqlContainer.StopAsync();
    }

    public async Task InitializeAsync()
    {
        await PostgreSqlContainer.StartAsync();

        var application = new MockApplication(PostgreSqlContainer.GetConnectionString());

        Client = application.CreateClient();
    }
}

public class MockApplication : WebApplicationFactory<Program>
{
    private readonly string _connectionString;
    public MockApplication(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            var memorySettings = new Dictionary<string, string?>
            {
                { "ConnectionStrings:DefaultConnection", _connectionString }
            };

            config.AddInMemoryCollection(memorySettings);
        });
    }
}

[CollectionDefinition(nameof(WebApplicationFixtureCollection))]
public class WebApplicationFixtureCollection : ICollectionFixture<WebApplicationFixture>
{
}