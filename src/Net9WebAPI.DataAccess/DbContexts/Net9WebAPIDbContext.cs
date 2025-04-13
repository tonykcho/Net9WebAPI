using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Net9WebAPI.Domain.Abstract;

namespace Net9WebAPI.DataAccess.DbContexts;
public class Net9WebAPIDbContext : DbContext
{
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
    // }
    public Net9WebAPIDbContext(DbContextOptions<Net9WebAPIDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Net9WebAPIDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var entry in ChangeTracker.Entries<BaseModel>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Guid = Guid.NewGuid();
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                    entry.Entity.LastUpdatedAt = DateTimeOffset.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastUpdatedAt = DateTimeOffset.UtcNow;
                    break;
            }
        }

        int result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}