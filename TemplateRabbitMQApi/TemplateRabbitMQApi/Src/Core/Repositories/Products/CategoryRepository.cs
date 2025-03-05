using TemplateRabbitMQApi.Core.Models.Products;
using TemplateRabbitMQApi.Core.Models;
using System.Data;

namespace TemplateRabbitMQApi.Core.Repositories.Products;

public class CategoryRepository : RepositoryBase<Category>
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
