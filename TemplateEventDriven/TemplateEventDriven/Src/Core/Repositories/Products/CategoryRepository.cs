using TemplateEventDriven.Core.Models.Products;
using TemplateEventDriven.Core.Models;
using System.Data;

namespace TemplateEventDriven.Core.Repositories.Products;

public class CategoryRepository : RepositoryBase<Category>
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
