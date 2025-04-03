using TemplateMongoDbApi.Core.Models.Products;
using MongoDB.Driver;

namespace TemplateMongoDbApi.Core.Repositories.Products;

public class CategoryRepository : MongoRepository<Category>
{
    public CategoryRepository(IMongoDatabase database) : base(database, "categories")
    {
    }
}
