using TemplateNatsApi.Core.Models.Statistics;
using TemplateNatsApi.Core.Repositories.Statistics;

namespace TemplateNatsApi.Core.Services.Statistics;

public class SupplierStatisticsService
{
    private readonly SupplierStatisticsRepository _repository;

    public SupplierStatisticsService(SupplierStatisticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SupplierTopSuppliers>> GetTopSuppliers()
    {
        return await _repository.GetTopSupplierCategoriesAsync();
    }
}