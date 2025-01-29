using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TemplateSimpleMVC.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    [DisplayName("Product Name")]
    public string? Name { get; set; }

    [Required]
    [DisplayName("Code")]
    public string? Code { get; set; }

    [Required]
    [DisplayName("Description")]
    public string? Description { get; set; }
}
