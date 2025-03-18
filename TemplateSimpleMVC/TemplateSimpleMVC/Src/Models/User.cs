using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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

    public static void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity => {
            entity.HasIndex(u => u.UserName)
                .IsUnique();
        });
    }
}
