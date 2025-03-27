using Microsoft.EntityFrameworkCore;
using Net9WebAPI.DataAccess.DbContexts;

namespace Net9WebAPI.WebAPI.Extensions;

public static class WebApplicationExtensions
{
    public static async Task MigrateAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        await using (var scope = app.Services.CreateAsyncScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();

            var context = scope.ServiceProvider.GetRequiredService<Net9WebAPIDbContext>();

            try
            {
                logger.LogInformation("--> Start migrating database.");

                await context.Database.MigrateAsync(cancellationToken);
                
                logger.LogInformation("--> Migrate database success.");
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message, "--> An Error occurred while migrating the database used on context.");
                throw;
            }
        }
    } 
}