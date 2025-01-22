using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [Route("")]
    [Route("/api")]
    [ApiController]
    public class ApiRootController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ApiRootController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var path = _configuration["TemplatesPath"] ?? string.Empty;
            var templatePath = Path.Combine(path, "Root/index.html");
            if (!System.IO.File.Exists(templatePath))
            {
                return NotFound("Template file not found");
            }
            var htmlContent = System.IO.File.ReadAllText(templatePath, Encoding.UTF8);
            return Content(htmlContent, "text/html", Encoding.UTF8);
        }

        [HttpGet("download-collections")]
        public IActionResult DownloadCollections()
        {
            var fileName = "postman.postman_collection.json";
            var path = _configuration["ApiCollectionsPath"] ?? string.Empty;
            var filePath = Path.Combine(path, fileName);
            
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Api collection file not found");
            }
            return new FileContentResult(System.IO.File.ReadAllBytes(filePath), "application/json")
            {
                FileDownloadName = fileName
            };
        }

    }
}
