using TemplateApi.Core.Models.Statistics;
using TemplateApi.Core.Repositories.Statistics;

namespace TemplateApi.Core.Services.Statistics
{
    public class ProductStatisticsService
    {
        private readonly ProductStatisticsRepository _repository;

        public ProductStatisticsService(ProductStatisticsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductTotalPriceByCategories>> GetProductTotalPriceByCategories()
        {
            return await _repository.GetProductTotalPriceByCategoriesAsync();
        }

        public async Task<IEnumerable<ProductTotalPriceBySuppliers>> GetProductTotalPriceBySuppliers()
        {
            return await _repository.GetProductTotalPriceBySuppliersAsync();
        }
    }
}