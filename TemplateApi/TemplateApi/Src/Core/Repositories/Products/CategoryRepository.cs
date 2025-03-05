using TemplateApi.Core.Models.Products;
using TemplateApi.Core.Models;
using System.Data;

namespace TemplateApi.Core.Repositories.Products;

public class CategoryRepository : RepositoryBase<Category>
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
