using TemplateMongoDbApi.Core.Models.Suppliers;
using TemplateMongoDbApi.Core.Models;
using System.Data;

namespace TemplateMongoDbApi.Core.Repositories.Suppliers;

public class SupplierRepository : MongoRepository<Supplier>
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
