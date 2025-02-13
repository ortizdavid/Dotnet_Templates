namespace TemplateMVC.Core.Repositories;

public interface IRepository<T> where T : class
{
    Task CreateAsync(T entity);
    Task CreateBatchAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task UpdateBatchAsync(IEnumerable<T> entities);
    Task DeleteAsync(T entity);
    Task<IEnumerable<T>> GetAllAsync(int pageSize, int pageIndex);
    Task<T?> GetByIdAsync(int id);
    Task<T?> GetByUniqueIdAsync(Guid uniqueId);
    Task<T?> GetByFieldAsync(string field, object value);
    Task<bool> ExistsRecord(string field, string value);
    Task<int> CountAsync();
}
