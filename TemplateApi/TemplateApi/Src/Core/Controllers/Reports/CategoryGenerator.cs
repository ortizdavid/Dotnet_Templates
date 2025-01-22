using System.Text;
using OfficeOpenXml;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using TemplateApi.Core.Models.Reports;

namespace TemplateApi.Core.Controllers.Reports
{
    public class CategoryGenerator : IGenerator<CategoryReport>
    {
        public byte[] GenerateExcel(ReportResponse<CategoryReport> response)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Categorys");
                // Add headers
                worksheet.Cells["A1"].Value = "Category ID";
                worksheet.Cells["B1"].Value = "Category Name";
                worksheet.Cells["C1"].Value = "Description";
                worksheet.Cells["D1"].Value = "Created At";
                // Add data
                int row = 2;
                foreach (var category in response.Items)
                {
                    worksheet.Cells[row, 1].Value = category.CategoryId;
                    worksheet.Cells[row, 2].Value = category.CategoryName;
                    worksheet.Cells[row, 3].Value = category.Description;
                    worksheet.Cells[row, 4].Value = category.CreatedAt.ToString("yyyy-MM-dd");
                    row++;
                }
                // Auto fit columns for better readability
                worksheet.Cells.AutoFitColumns();
                // Convert Excel package to bytes
                return package.GetAsByteArray();
            }
        }
        public byte[] GenerateCSV(ReportResponse<CategoryReport> response)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                {
                    streamWriter.WriteLine("CategoryId,CategoryName,Description,CreatedAt");
                    // Write data
                    foreach (var category in response.Items)
                    {
                        streamWriter.WriteLine(string.Format(
                            "{0},{1},{2},{3}",
                            category.CategoryId,
                            category.CategoryName,
                            category.Description,
                            category.CreatedAt.ToString("yyyy-MM-dd")
                        ));
                    }
                    streamWriter.Flush();
                }
                return memoryStream.ToArray();
            }
        }
        public byte[] GeneratePDF(ReportResponse<CategoryReport> response)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics graphics = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 7); // Adjusted font size to 7
                // Draw headers
                graphics.DrawString("Category ID", font, XBrushes.Black, new XRect(30, 50, 100, 20), XStringFormats.TopLeft);
                graphics.DrawString("Category Name", font, XBrushes.Black, new XRect(150, 50, 100, 20), XStringFormats.TopLeft);
                graphics.DrawString("Description", font, XBrushes.Black, new XRect(250, 50, 100, 20), XStringFormats.TopLeft);
                graphics.DrawString("Created At", font, XBrushes.Black, new XRect(350, 50, 150, 20), XStringFormats.TopLeft);
                // Draw data
                int yPosition = 70;
                foreach (var category in response.Items)
                {
                    graphics.DrawString(category.CategoryId.ToString() ?? "N/A", font, XBrushes.Black, new XRect(30, yPosition, 100, 20), XStringFormats.TopLeft);
                    graphics.DrawString(category.CategoryName ?? "N/A", font, XBrushes.Black, new XRect(150, yPosition, 100, 20), XStringFormats.TopLeft);
                    graphics.DrawString(category.Description ?? "N/A", font, XBrushes.Black, new XRect(250, yPosition, 100, 20), XStringFormats.TopLeft);
                    graphics.DrawString(category.CreatedAt.ToString() ?? "N/A", font, XBrushes.Black, new XRect(350, yPosition, 150, 20), XStringFormats.TopLeft);
                    yPosition += 20;
                }
                document.Save(memoryStream);
                return memoryStream.ToArray();
            }
        }

    }
}
    