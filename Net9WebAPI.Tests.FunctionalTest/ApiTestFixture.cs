
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Net9WebAPI.Tests.FunctionalTest
{
    public class ApiTestFixture : IDisposable
    {
        public HttpClient Client { get; set; }
        // This allow override the default WebApplicationFactory
        public class MockApplication : WebApplicationFactory<Program>
        {
            protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                base.ConfigureWebHost(builder);

                builder.ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.test.json");
                });
            }
        }

        public ApiTestFixture()
        {
            var app = new MockApplication();
            Client = app.CreateClient();
        }

        public void Dispose()
        {
        }
    }
}