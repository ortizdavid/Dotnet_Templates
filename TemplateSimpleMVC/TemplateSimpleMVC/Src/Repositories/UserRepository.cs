using Microsoft.EntityFrameworkCore;
using TemplateSimpleMVC.Models;

namespace TemplateSimpleMVC.Repositories;

public class UserRepository : IRepository<User>
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public int Count()
    {
        return _context.Users.Count();
    }

    public void Create(User entity)
    {
        _context.Users.Add(entity);
        _context.SaveChanges();
    }

    public void Delete(User entity)
    {
        _context.Users.Remove(entity);
        _context.SaveChanges();
    }

    public bool Exists(string field, string? value)
    {
        return _context.Users.Any(u => EF.Property<string>(u, field) == value);
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users.ToList();
    }

    public User? GetById(int id)
    {
        return _context.Users.Find(id);
    }

    public void Update(User entity)
    {
        _context.Update(entity);
        _context.SaveChanges();
    }

    public User? FindByName(string? name)
    {
        return _context.Users.AsNoTracking().FirstOrDefault(u => u.UserName == name);
    }
}