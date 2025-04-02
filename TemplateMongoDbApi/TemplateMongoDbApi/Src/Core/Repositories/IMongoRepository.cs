namespace TemplateMongoDbApi.Core.Repositories;

public interface IMongoRepository<T> where T : class
{
    Task CreateAsync(T entity);
    Task CreateBatchAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task UpdateBatchAsync(IEnumerable<T> entities);
    Task DeleteAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync(int pageSize, int pageIndex);
    Task<T?> GetByIdAsync(string id);
    Task<T?> GetByUniqueIdAsync(Guid uniqueId);
    Task<T?> GetByFieldAsync(string field, object value);
    Task<bool> ExistsRecord(string field, string value);
    Task<long> CountAsync();
}
