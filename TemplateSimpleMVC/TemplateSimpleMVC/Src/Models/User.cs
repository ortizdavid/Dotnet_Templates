using System.ComponentModel.DataAnnotations;

namespace TemplateSimpleMVC.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)] 
    public string? UserName { get; set; }

    [Required]
    [StringLength(150)]  
    public string? Password { get; set; }
}
