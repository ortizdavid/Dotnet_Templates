using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Net;
using TemplateApi.Core.Models.Reports;

namespace TemplateApi.Core.Controllers.Reports;

public class ReportFormat
{
    public static IActionResult Handle<T>(IGenerator<T> generator, ReportResponse<T> items, string format, string fileName) where T : class 
    {
        format = format?.ToLower() ?? "json";
        string extension = format;
        string contentType;
        byte[] data;

        if (format == "json")
        {
            return new OkObjectResult(items);
        }

        switch (format)
        {
            case "csv":
                contentType = "text/csv";
                data = generator.GenerateCSV(items);
                break;
            case "excel":
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                data = generator.GenerateExcel(items);
                extension = "xlsx";
                break;
            case "pdf":
                contentType = "application/pdf";
                data = generator.GeneratePDF(items);
                break;
            default:
                return new BadRequestObjectResult($"Unsupported format '{format}'. Supported formats are: json, excel, pdf, csv.");  
        }
        return new FileContentResult(data, contentType)
        {
            FileDownloadName = $"{fileName}_Report.{extension}"
        };
    }
}