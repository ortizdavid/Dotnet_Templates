namespace TemplateSimpleApi.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Create(T model);
        IEnumerable<T> GetAll();
        T? GetById(int id);
        void Update(T model);
        void Delete(T model);
        bool ExistsRecord(string field, string? value);
    }
}