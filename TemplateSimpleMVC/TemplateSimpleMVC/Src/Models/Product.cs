using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TemplateSimpleMVC.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    [DisplayName("Product Name")]
    [StringLength(50)] 
    public string? Name { get; set; }

    [Required]
    [DisplayName("Code")]
    [StringLength(20)] 
    public string? Code { get; set; }

    [Required]
    [DisplayName("Price")]
    [DisplayFormat(DataFormatString = "{0:C0}")]
    public decimal Price { get; set; }

    [Required]
    [DisplayName("Description")]
    public string? Description { get; set; }
}
