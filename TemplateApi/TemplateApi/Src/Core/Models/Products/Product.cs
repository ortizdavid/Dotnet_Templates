using System.ComponentModel.DataAnnotations;
using TemplateApi.Helpers;

namespace TemplateApi.Core.Models.Products
{   
    public class Product
    {   
        [Key]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Code { get; set; }
        public decimal UnitPrice { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public Guid UniqueId { get; set; } = Encryption.GenerateUUID();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
