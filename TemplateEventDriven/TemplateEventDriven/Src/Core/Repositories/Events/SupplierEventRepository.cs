using TemplateEventDriven.Core.Models;
using TemplateEventDriven.Core.Models.Events;

namespace TemplateEventDriven.Core.Repositories.Events;

public class SupplierEventRepository : RepositoryBase<SupplierEvent>
{
    private readonly AppDbContext _context;

    public SupplierEventRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}