using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TemplateSimpleApi.Models;

namespace TemplateSimpleApi.Repositories;

public class ProductRepository : IRepository<Product>
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Create(Product model)
    {
        _context.Products.Add(model);
        _context.SaveChanges();
    }

    public void Delete(Product model)
    {
        _context.Products.Remove(model);
        _context.SaveChanges();
    }

    public bool ExistsRecord(string field, string? value)
    {
        return _context.Products.Any(p => EF.Property<string>(p, field) == value);
    }

    public IEnumerable<Product> GetAll()
    {
        return _context.Products.ToList();
    }

    public Product? GetById(int id)
    {
        return _context.Products.FirstOrDefault(p => p.Id == id);
    }

    public void Update(Product model)
    {
        _context.Products.Update(model);
        _context.SaveChanges();
    }
}