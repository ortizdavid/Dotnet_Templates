using TemplateEventDriven.Core.Models.Statistics;
using TemplateEventDriven.Core.Repositories.Statistics;

namespace TemplateEventDriven.Core.Services.Statistics;

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