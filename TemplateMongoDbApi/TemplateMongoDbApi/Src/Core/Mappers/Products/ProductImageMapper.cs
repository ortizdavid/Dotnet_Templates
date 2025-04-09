using PdfSharp.Snippets.Drawing;
using TemplateMongoDbApi.Core.Models.Products;

namespace TemplateMongoDbApi.Core.DTOs.Products;

public static class ProductImageMapper
{
    public static ProductImageResponse ToResponse(ProductImage productImage)
    {
        return new ProductImageResponse
        {
            ImageId = productImage.ImageId.ToString(),
            FileName = productImage.FileName,
            UploadDir = productImage.UploadDir,
            CreatedAt = productImage.CreatedAt,
            UpdatedAt = productImage.UpdatedAt
        };
    }

    public static List<ProductImageResponse> ToResponseList(IEnumerable<ProductImage> images)
    {
        return images.Select(im => new ProductImageResponse
        {   
            ImageId = im.ImageId.ToString(),
            FileName = im.FileName,
            UploadDir = im.UploadDir,
            CreatedAt = im.CreatedAt,
            UpdatedAt = im.UpdatedAt
        }).ToList();
    }
}