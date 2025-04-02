using TemplateMongoDbApi.Core.Models.Products;
using TemplateMongoDbApi.Core.Models;
using System.Data;

namespace TemplateMongoDbApi.Core.Repositories.Products;

public class CategoryRepository : RepositoryBase<Category>
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
