using TemplateMVC.Core.Models.Products;
using Microsoft.EntityFrameworkCore;
using TemplateMVC.Core.Models;

namespace TemplateMVC.Core.Repositories.Products;

public class CategoryRepository : RepositoryBase<Category>
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Category?> GetByNameAsync(string? name)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.CategoryName == name);
    }
}
