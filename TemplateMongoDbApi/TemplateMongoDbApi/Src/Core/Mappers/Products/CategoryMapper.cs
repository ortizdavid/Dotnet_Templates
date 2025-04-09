using TemplateMongoDbApi.Core.DTOs.Products;
using TemplateMongoDbApi.Core.Models.Products;

namespace TemplateMongoDbApi.Core.Mappers.Products;

public static class CategoryMapper
{
    public static CategoryResponse ToResponse(Category? category)
    {
        return new CategoryResponse()
        {
            CategoryId = category?.CategoryId.ToString(),
            CategoryName = category?.CategoryName,
            Description = category?.Description,
            CreatedAt = category?.CreatedAt,
            UpdatedAt = category?.UpdatedAt
        };
    }

    public static List<CategoryResponse> ToResponseList(IEnumerable<Category> categories)
    {
        return categories.Select(c => new CategoryResponse
        {
            CategoryId = c.CategoryId.ToString(),
            CategoryName = c.CategoryName,
            Description = c.Description,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();
    }
}