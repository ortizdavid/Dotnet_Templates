using TemplateEventDriven.Core.Models;
using TemplateEventDriven.Core.Models.Events;

namespace TemplateEventDriven.Core.Repositories.Events;

public class ProductEventRepository : RepositoryBase<ProductEvent>
{
    private readonly AppDbContext _context;

    public ProductEventRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}