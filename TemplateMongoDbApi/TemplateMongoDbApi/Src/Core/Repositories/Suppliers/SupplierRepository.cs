using TemplateMongoDbApi.Core.Models.Suppliers;
using TemplateMongoDbApi.Core.Models;
using System.Data;
using MongoDB.Driver;

namespace TemplateMongoDbApi.Core.Repositories.Suppliers;

public class SupplierRepository : MongoRepository<Supplier>
{
    public SupplierRepository(IMongoDatabase database) : base(database, "suppliers")
    {
    }

     public async Task<Supplier> GetByNameAsync(string name)
    {
        var filter = _builder.Eq(c => c.SupplierName, name);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}
