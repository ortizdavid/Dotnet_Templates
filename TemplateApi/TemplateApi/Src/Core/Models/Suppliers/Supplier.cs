using System.ComponentModel.DataAnnotations;
using TemplateApi.Helpers;

namespace TemplateApi.Core.Models.Suppliers
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        public string? SupplierName { get; set; }

        public string? IdentificationNumber { get; set; }

        public string? Email { get; set; }  

        public string? PrimaryPhone { get; set; } 

        public string? SecondaryPhone { get; set; } 

        public string? Address { get; set; }

        public Guid UniqueId { get; set; } = Encryption.GenerateUUID();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set;} = DateTime.UtcNow;
    }
}
