using TemplateNatsApi.Core.Models.Reports;

namespace TemplateNatsApi.Core.Controllers.Reports;

public interface IGenerator <T> where T : class
{
    byte[] GeneratePDF(ReportResponse<T> response);
    byte[] GenerateExcel(ReportResponse<T> response);
    byte[] GenerateCSV(ReportResponse<T> response);
}