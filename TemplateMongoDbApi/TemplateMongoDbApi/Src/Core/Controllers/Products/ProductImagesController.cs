using Microsoft.AspNetCore.Mvc;

namespace TemplateMongoDbApi.Core.Controllers.Products;

[ApiController]
[Route("api/product-images")]
public class ProductImagesController : ControllerBase
{
    private readonly string _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Uploads", "Products");

    [HttpGet("{fileName}")]
    public IActionResult GetImageByFileName(string fileName)
    {
        var filePath = Path.Combine(_imageDirectory, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("Image not found.");
        }

        var contentType = "image/png"; 

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, contentType);
    }
}
