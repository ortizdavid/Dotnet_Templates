using Microsoft.EntityFrameworkCore;
using TemplateSimpleMVC.Models;

namespace TemplateSimpleMVC.Repositories;

public class ProductRepository : IRepository<Product>
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public int Count()
    {
        return _context.Products.Count();
    }

    public void Create(Product entity)
    {
        _context.Products.Add(entity);
        _context.SaveChanges();
    }

    public void Delete(Product entity)
    {
        _context.Products.Remove(entity);
        _context.SaveChanges();
    }

    public bool Exists(string field, string? value)
    {
        return _context.Products.Any(p => EF.Property<string>(p, field) == value);
    }

    public IEnumerable<Product> GetAll()
    {
        return _context.Products.ToList();
    }

    public IEnumerable<Product> GetAllLimit(int limit, int offset)
    {
        return _context.Products
            .Skip(offset) 
            .Take(limit)
            .ToList();
    }

    public Product? GetById(int id)
    {
        return _context.Products.Find(id);
    }

    public void Update(Product entity)
    {
        _context.Update(entity);
        _context.SaveChanges();
    }
}