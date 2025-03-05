using TemplateRabbitMQApi.Core.Models.Suppliers;
using TemplateRabbitMQApi.Core.Models;
using System.Data;

namespace TemplateRabbitMQApi.Core.Repositories.Suppliers;

public class SupplierRepository : RepositoryBase<Supplier>
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}
