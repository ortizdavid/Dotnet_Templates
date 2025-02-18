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
        return await _dbSet
            .FirstOrDefaultAsync(s => s.IdentificationNumber == identNumber);    
    }

    public async Task<bool> ExistsRecordExcluded(string identNumber, string email, string phone1, string phone2, Guid excludedId)
    {
        return await _dbSet.AnyAsync(s => 
            (s.IdentificationNumber == identNumber || s.Email == email || s.PrimaryPhone == phone1 || s.SecondaryPhone == phone2) 
            && s.UniqueId == excludedId
        );
    }
}
