using System.Text;
using OfficeOpenXml;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using TemplateMVC.Core.Models.Reports;
using TemplateMVC.Core.Models.Suppliers;

namespace TemplateMVC.Core.Controllers.Reports;

public class SupplierGenerator : IGenerator<SupplierReport>
{
    public byte[] GenerateExcel(ReportResponse<SupplierReport> response)
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Suppliers");
            // Add headers
            worksheet.Cells["A1"].Value = "Supplier ID";
            worksheet.Cells["B1"].Value = "Supplier Name";
            worksheet.Cells["C1"].Value = "Identification Number";
            worksheet.Cells["D1"].Value = "Email";
            worksheet.Cells["E1"].Value = "Primary Phone";
            worksheet.Cells["F1"].Value = "Secondary Phone";
            worksheet.Cells["G1"].Value = "Address";
            worksheet.Cells["H1"].Value = "Created At";
            // Add data
            int row = 2;
            foreach (var product in response.Items)
            {
                worksheet.Cells[row, 1].Value = product.SupplierId;
                worksheet.Cells[row, 2].Value = product.SupplierName;
                worksheet.Cells[row, 3].Value = product.IdentificationNumber;
                worksheet.Cells[row, 4].Value = product.Email;
                worksheet.Cells[row, 5].Value = product.PrimaryPhone;
                worksheet.Cells[row, 6].Value = product.SecondaryPhone;
                worksheet.Cells[row, 7].Value = product.Address;
                worksheet.Cells[row, 8].Value = product.CreatedAt.ToString("yyyy-MM-dd");
                row++;
            }
            // Auto fit columns for better readability
            worksheet.Cells.AutoFitColumns();
            // Convert Excel package to bytes
            return package.GetAsByteArray();
        }
    }
    public byte[] GenerateCSV(ReportResponse<SupplierReport> response)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                streamWriter.WriteLine("SupplierId,SupplierName,IdentificationNumber,Email,PrimaryPhone,SecondaryPhone,Address,CreatedAt");
                // Write data
                foreach (var product in response.Items)
                {
                    streamWriter.WriteLine(string.Format(
                        "{0},{1},{2},{3},{4},{5},{6},{7}",
                        product.SupplierId,
                        product.SupplierName,
                        product.IdentificationNumber,
                        product.Email,
                        product.PrimaryPhone,
                        product.SecondaryPhone,
                        product.Address,
                        product.CreatedAt.ToString("yyyy-MM-dd")
                    ));
                }
                streamWriter.Flush();
            }
            return memoryStream.ToArray();
        }
    }
   
    public byte[] GeneratePDF(ReportResponse<SupplierReport> response)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics graphics = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Arial", 7); // Adjusted font size to 7
            // Draw headers
            graphics.DrawString("Supplier ID", font, XBrushes.Black, new XRect(30, 50, 100, 20), XStringFormats.TopLeft);
            graphics.DrawString("Supplier Name", font, XBrushes.Black, new XRect(150, 50, 100, 20), XStringFormats.TopLeft);
            graphics.DrawString("Identification Number", font, XBrushes.Black, new XRect(250, 50, 100, 20), XStringFormats.TopLeft);
            graphics.DrawString("Phone", font, XBrushes.Black, new XRect(350, 50, 150, 20), XStringFormats.TopLeft);
            graphics.DrawString("Email", font, XBrushes.Black, new XRect(450, 50, 150, 20), XStringFormats.TopLeft);
            graphics.DrawString("Address", font, XBrushes.Black, new XRect(550, 50, 100, 20), XStringFormats.TopLeft);
            // Draw data
            int yPosition = 70;
            foreach (var product in response.Items)
            {
                graphics.DrawString(product.SupplierId.ToString() ?? "N/A", font, XBrushes.Black, new XRect(30, yPosition, 100, 20), XStringFormats.TopLeft);
                graphics.DrawString(product.SupplierName ?? "N/A", font, XBrushes.Black, new XRect(150, yPosition, 100, 20), XStringFormats.TopLeft);
                graphics.DrawString(product.IdentificationNumber ?? "N/A", font, XBrushes.Black, new XRect(250, yPosition, 100, 20), XStringFormats.TopLeft);
                graphics.DrawString(product.PrimaryPhone ?? "N/A", font, XBrushes.Black, new XRect(350, yPosition, 150, 20), XStringFormats.TopLeft);
                graphics.DrawString(product.Email ?? "N/A", font, XBrushes.Black, new XRect(450, yPosition, 150, 20), XStringFormats.TopLeft);
                graphics.DrawString(product.Address ?? "NA", font, XBrushes.Black, new XRect(550, yPosition, 100, 20), XStringFormats.TopLeft);
                yPosition += 20;
            }
            document.Save(memoryStream);
            return memoryStream.ToArray();
        }
    }

}
