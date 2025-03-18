namespace TemplateSimpleMVC.Repositories;

public interface IRepository<T> where T : class
{
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    IEnumerable<T> GetAll();
    T? GetById(int id);
    bool Exists(string field, string value);
    int Count();
}