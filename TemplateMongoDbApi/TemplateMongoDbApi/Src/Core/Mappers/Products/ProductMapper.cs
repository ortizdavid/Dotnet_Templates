using TemplateMongoDbApi.Core.DTOs.Products;
using TemplateMongoDbApi.Core.Mappers.Suppliers;
using TemplateMongoDbApi.Core.Models.Products;

namespace TemplateMongoDbApi.Core.Mappers.Products;

public static class ProductMapper
{
    public static ProductResponse ToResponse(Product? product)
    {
        return new ProductResponse
        {
            ProductId = product?.ProductId.ToString(),
            ProductName = product?.ProductName,
            Code = product?.Code,
            UnitPrice = product?.UnitPrice,
            Description = product?.Description,
            CreatedAt = product?.CreatedAt,
            UpdatedAt = product?.UpdatedAt,
            Category = CategoryMapper.ToResponse(product?.Category),
            Supplier = SupplierMapper.ToResponse(product?.Supplier)
        };
    }

    public static List<ProductResponse> ToResponseList(IEnumerable<Product> products)
    {
        return products.Select(p => new ProductResponse
        {
            ProductId = p.ProductId.ToString(),
            ProductName = p.ProductName,
            Code = p.Code,
            UnitPrice = p.UnitPrice,
            Description = p.Description,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            Category = CategoryMapper.ToResponse(p.Category),
            Supplier = SupplierMapper.ToResponse(p?.Supplier)
        }).ToList();
    }
}

