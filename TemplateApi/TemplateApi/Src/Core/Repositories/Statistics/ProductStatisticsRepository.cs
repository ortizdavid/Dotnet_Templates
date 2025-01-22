using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using TemplateApi.Core.Models.Statistics;

namespace TemplateApi.Core.Repositories.Statistics
{
    public class ProductStatisticsRepository
    {
        private readonly IDbConnection _dapper;

        public ProductStatisticsRepository(IDbConnection dapper)
        {
            _dapper = dapper;
        }

        public async Task<IEnumerable<ProductTotalPriceByCategories>> GetProductTotalPriceByCategoriesAsync()
        {
            var sql = "SELECT * FROM ViewProductTotalPriceByCategories;";
            return await _dapper.QueryAsync<ProductTotalPriceByCategories>(sql);
        }

        public async Task<IEnumerable<ProductTotalPriceBySuppliers>> GetProductTotalPriceBySuppliersAsync()
        {
            var sql = "SELECT * FROM ViewProductTotalPriceBySuppliers;";
            return await _dapper.QueryAsync<ProductTotalPriceBySuppliers>(sql);
        }
    }
}
