using TemplateRedisApi.Core.Models.Products;
using TemplateRedisApi.Core.Models;
using System.Data;

namespace TemplateRedisApi.Core.Repositories.Products;

public class CategoryRepository : RepositoryBase<Category>
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
