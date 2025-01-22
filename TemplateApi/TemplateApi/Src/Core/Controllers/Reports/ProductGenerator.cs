using System.Text;
using OfficeOpenXml;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using TemplateApi.Core.Models.Reports;

namespace TemplateApi.Core.Controllers.Reports
{
    public class ProductGenerator : IGenerator<ProductReport>
    {
        public byte[] GenerateExcel(ReportResponse<ProductReport> response)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Products");
                // Add headers
                worksheet.Cells["A1"].Value = "Product ID";
                worksheet.Cells["B1"].Value = "Product Name";
                worksheet.Cells["C1"].Value = "Code";
                worksheet.Cells["D1"].Value = "Unit Price";
                worksheet.Cells["E1"].Value = "Category Name";
                worksheet.Cells["F1"].Value = "Description";
                worksheet.Cells["G1"].Value = "Created At";
                // Add data
                int row = 2;
                foreach (var product in response.Items)
                {
                    worksheet.Cells[row, 1].Value = product.ProductId;
                    worksheet.Cells[row, 2].Value = product.ProductName;
                    worksheet.Cells[row, 3].Value = product.Code;
                    worksheet.Cells[row, 4].Value = product.UnitPrice;
                    worksheet.Cells[row, 5].Value = product.CategoryName;
                    worksheet.Cells[row, 6].Value = product.Description;
                    worksheet.Cells[row, 7].Value = product.CreatedAt.ToString("yyyy-MM-dd");
                    row++;
                }
                // Auto fit columns for better readability
                worksheet.Cells.AutoFitColumns();
                // Convert Excel package to bytes
                return package.GetAsByteArray();
            }
        }
        public byte[] GenerateCSV(ReportResponse<ProductReport> response)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                {
                    streamWriter.WriteLine("ProductId,ProductName,Code,UnitPrice,CategoryName,Description,CreatedAt");
                    // Write data
                    foreach (var product in response.Items)
                    {
                        streamWriter.WriteLine(string.Format(
                            "{0},{1},{2},{3},{4},{5},{6},{7}",
                            product.ProductId,
                            product.ProductName,
                            product.Code,
                            product.UnitPrice,
                            product.CategoryName,
                            product.Description,
                            product.CreatedAt.ToString("yyyy-MM-dd")
                        ));
                    }
                    streamWriter.Flush();
                }
                return memoryStream.ToArray();
            }
        }
        public byte[] GeneratePDF(ReportResponse<ProductReport> response)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics graphics = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 7); // Adjusted font size to 7
                // Draw headers
                graphics.DrawString("Product ID", font, XBrushes.Black, new XRect(30, 50, 100, 20), XStringFormats.TopLeft);
                graphics.DrawString("Product Name", font, XBrushes.Black, new XRect(150, 50, 100, 20), XStringFormats.TopLeft);
                graphics.DrawString("Code", font, XBrushes.Black, new XRect(250, 50, 100, 20), XStringFormats.TopLeft);
                graphics.DrawString("Category Name", font, XBrushes.Black, new XRect(350, 50, 150, 20), XStringFormats.TopLeft);
                graphics.DrawString("Unit Price", font, XBrushes.Black, new XRect(450, 50, 150, 20), XStringFormats.TopLeft);
                graphics.DrawString("CreatedAt", font, XBrushes.Black, new XRect(550, 50, 100, 20), XStringFormats.TopLeft);
                // Draw data
                int yPosition = 70;
                foreach (var product in response.Items)
                {
                    graphics.DrawString(product.ProductId.ToString() ?? "N/A", font, XBrushes.Black, new XRect(30, yPosition, 100, 20), XStringFormats.TopLeft);
                    graphics.DrawString(product.ProductName ?? "N/A", font, XBrushes.Black, new XRect(150, yPosition, 100, 20), XStringFormats.TopLeft);
                    graphics.DrawString(product.Code ?? "N/A", font, XBrushes.Black, new XRect(250, yPosition, 100, 20), XStringFormats.TopLeft);
                    graphics.DrawString(product.CategoryName ?? "N/A", font, XBrushes.Black, new XRect(350, yPosition, 150, 20), XStringFormats.TopLeft);
                    graphics.DrawString(product.UnitPrice?.ToString("F2") ?? "0.00", font, XBrushes.Black, new XRect(450, yPosition, 150, 20), XStringFormats.TopLeft);
                    graphics.DrawString(product.CreatedAt.ToString("yyyy-MM-dd"), font, XBrushes.Black, new XRect(550, yPosition, 100, 20), XStringFormats.TopLeft);
                    yPosition += 20;
                }
                document.Save(memoryStream);
                return memoryStream.ToArray();
            }
        }

    }
}
    