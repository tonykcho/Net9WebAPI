using Microsoft.EntityFrameworkCore.Storage;
using Net9WebAPI.Domain.Abstract;

namespace Net9WebAPI.DataAccess.Abstract;

public interface IBaseRepository<T> where T : BaseModel
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<T?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken);
    Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken);
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    void Update(T entity);
    void Remove(T entity);
}