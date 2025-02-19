using TemplateMVC.Core.Models.Suppliers;
using Microsoft.EntityFrameworkCore;
using TemplateMVC.Core.Models;
using System.Data;
using Dapper;

namespace TemplateMVC.Core.Repositories.Suppliers;

public class SupplierRepository : RepositoryBase<Supplier>
{
    private readonly AppDbContext _context;
    private readonly IDbConnection _dapper;

    private static readonly Dictionary<string, string> _sortOptions = new()
    {
        ["name_asc"] = "SupplierName ASC",
        ["name_desc"] = "SupplierName DESC",
        ["ident_asc"] = "IdentificationNumber ASC",
        ["ident_desc"] = "IdentificationNumber DESC"
    };

    public SupplierRepository(AppDbContext context, IDbConnection dapper) : base(context)
    {
        _context = context;
        _dapper = dapper;
    }

    public async Task<IEnumerable<Supplier>> GetAllStortedAsync(int pageSize, int pageIndex, string searchString, string sortOrder)
    {
        int offset = pageIndex * pageSize;
        var parameters = new DynamicParameters();
        var filter = "";

        if (!string.IsNullOrEmpty(searchString))
        {
            filter = "WHERE SupplierName LIKE @SearchParam OR IdentificationNumber LIKE @SearchParam";
            parameters.Add("SearchParam", $"%{searchString ?? ""}%");
        } 

        var sort = _sortOptions.GetValueOrDefault(sortOrder, "SupplierId ASC");
        var sql = @$"SELECT * FROM Suppliers 
                    {filter} ORDER BY {sort} 
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

        parameters.Add("Offset", offset);
        parameters.Add("PageSize", pageSize);
        return await _dapper.QueryAsync<Supplier>(sql, parameters);
    }

    public async Task<Supplier?> GetByIdentNumberAsync(string? identNumber)
    {
        return await _dbSet
            .FirstOrDefaultAsync(s => s.IdentificationNumber == identNumber);    
    }

    public async Task<bool> ExistsRecordExcluded(string? identNumber, string? email, string? phone1, string? phone2, Guid excludedId)
    {
        return await _dbSet.AnyAsync(s => 
            (s.IdentificationNumber == identNumber || s.Email == email || s.PrimaryPhone == phone1 || s.SecondaryPhone == phone2) 
            && s.UniqueId != excludedId
        );
    }
}
