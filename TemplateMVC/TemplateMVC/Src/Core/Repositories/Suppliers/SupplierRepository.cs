using TemplateMVC.Core.Models.Suppliers;
using Microsoft.EntityFrameworkCore;
using TemplateMVC.Core.Models;

namespace TemplateMVC.Core.Repositories.Suppliers;

public class SupplierRepository : RepositoryBase<Supplier>
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Supplier?> GetByIdentNumberAsync(string? identNumber)
    {
        return await _context.Suppliers
            .FirstOrDefaultAsync(s => s.IdentificationNumber == identNumber);    
    }
}
