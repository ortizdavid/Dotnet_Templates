using TemplateRabbitMQApi.Core.Models.Statistics;
using TemplateRabbitMQApi.Core.Repositories.Statistics;

namespace TemplateRabbitMQApi.Core.Services.Statistics;

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