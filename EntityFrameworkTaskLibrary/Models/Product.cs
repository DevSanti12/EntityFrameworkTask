using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkTaskLibrary.Models;

public class Product
{
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    [Required]
    [MaxLength(100)]
    public string Description { get; set; }
    [Required]
    public float Weight { get; set; }
    [Required]
    public float Height { get; set; }
    [Required]
    public float Width { get; set; }
    [Required]
    public float Length { get; set; }
}
