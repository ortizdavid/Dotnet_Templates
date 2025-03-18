using System.ComponentModel.DataAnnotations;

namespace TemplateMinimalAPI.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string? Name { get; set; }

    [Required]
    [StringLength(20)]
    public string? Code { get; set; }

    [Required]
    public decimal Price { get; set; }
}