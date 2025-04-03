using TemplateRedisApi.Core.Models.Suppliers;
using TemplateRedisApi.Core.Models;
using System.Data;

namespace TemplateRedisApi.Core.Repositories.Suppliers;

public class SupplierRepository : RepositoryBase<Supplier>
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
