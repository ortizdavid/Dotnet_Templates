using TemplateEventDriven.Core.Models.Suppliers;
using TemplateEventDriven.Core.Models;
using System.Data;

namespace TemplateEventDriven.Core.Repositories.Suppliers;

public class SupplierRepository : RepositoryBase<Supplier>
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
