using TemplateNatsApi.Core.Models.Products;
using TemplateNatsApi.Core.Models;
using System.Data;

namespace TemplateNatsApi.Core.Repositories.Products;

public class CategoryRepository : RepositoryBase<Category>
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
