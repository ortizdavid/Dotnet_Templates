using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using TemplateMVC.Common.Exceptions;
using TemplateMVC.Core.Models.Reports;

namespace TemplateMVC.Core.Controllers.Reports;

public class ReportFormat
{
    public static IActionResult Handle<T>(IGenerator<T> generator, ReportResponse<T> items, string format, string fileName) where T : class 
    {
        format = format?.ToLower() ?? "excel";
        string extension = format;
        string contentType;
        byte[] data;

        switch (format)
        {
            case "excel":
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                data = generator.GenerateExcel(items);
                extension = "xlsx";
                break;
            case "csv":
                contentType = "text/csv";
                data = generator.GenerateCSV(items);
                break;
            case "pdf":
                contentType = "application/pdf";
                data = generator.GeneratePDF(items);
                break;
            case "json":
                contentType = "application/json";
                string jsonContent = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
                data = Encoding.UTF8.GetBytes(jsonContent);
                break;
            default:
                throw new BadRequestException($"Unsupported format '{format}'. Supported formats are: json, excel, pdf, csv.");  
        }
        var timestamp = DateTime.Now.ToString("dd-MM-yyyy-HHmmss");
        var fullFileName = $"{fileName}_Report_{timestamp}.{extension}";
        return new FileContentResult(data, contentType)
        {
            FileDownloadName = fullFileName
        };
    }
}