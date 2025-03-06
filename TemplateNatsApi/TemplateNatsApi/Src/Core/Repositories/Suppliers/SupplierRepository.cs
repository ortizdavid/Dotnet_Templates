using TemplateNatsApi.Core.Models.Suppliers;
using TemplateNatsApi.Core.Models;
using System.Data;

namespace TemplateNatsApi.Core.Repositories.Suppliers;

public class SupplierRepository : RepositoryBase<Supplier>
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
