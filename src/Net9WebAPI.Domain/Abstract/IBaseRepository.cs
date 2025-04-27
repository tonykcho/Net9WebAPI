namespace Net9WebAPI.Domain.Abstract;

public interface IBaseRepository<T> where T : BaseModel
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<T?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken);
    Task<IList<T>> ListAsync(CancellationToken cancellationToken);
    Task AddAsync(T entity, CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
    void Update(T entity);
    void Remove(T entity);
}