using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Net9WebAPI.DataAccess.Abstract;
using Net9WebAPI.DataAccess.DbContexts;
using Net9WebAPI.Domain.Abstract;

namespace Net9WebAPI.DataAccess.Repositories;

public abstract class BaseRepository<T>(Net9WebAPIDbContext dbContext) : IBaseRepository<T> where T : BaseModel
{
    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await dbContext.AddAsync(entity, cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task<T?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await dbContext.Set<T>().SingleOrDefaultAsync(data => data.Guid == guid, cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await dbContext.Set<T>().SingleOrDefaultAsync(data => data.Id == id, cancellationToken);
    }

    public async Task<IList<T>> ListAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await dbContext.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public void Update(T entity)
    {
        dbContext.Update(entity);
    }

    public void Remove(T entity)
    {
        dbContext.Remove(entity);
    }
}