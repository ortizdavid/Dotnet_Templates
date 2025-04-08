using TemplateMongoDbApi.Core.Models.Products;
using MongoDB.Driver;

namespace TemplateMongoDbApi.Core.Repositories.Products;

public class CategoryRepository : MongoRepository<Category>
{
    public CategoryRepository(IMongoDatabase database) : base(database, "categories")
    {
    }

    public async Task<Category> GetByNameAsync(string name)
    {
        var filter = _builder.Eq(c => c.CategoryName, name);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}
