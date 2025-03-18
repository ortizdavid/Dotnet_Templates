using TemplateEventDriven.Core.Models;
using TemplateEventDriven.Core.Models.Events;

namespace TemplateEventDriven.Core.Repositories.Events;

public class CategoryEventRepository : RepositoryBase<CategoryEvent>
{
    private readonly AppDbContext _context;

    public CategoryEventRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}