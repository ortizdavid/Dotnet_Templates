using TemplateApi.Core.Models.Suppliers;
using TemplateApi.Core.Models;
using System.Data;

namespace TemplateApi.Core.Repositories.Suppliers;

public class SupplierRepository : RepositoryBase<Supplier>
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
