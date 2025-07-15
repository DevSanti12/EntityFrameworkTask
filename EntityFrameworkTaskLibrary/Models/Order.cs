using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkTaskLibrary.Models;

public class Order
{
    public int Id { get; set; }
    [Required]
    public OrderStatus Status { get; set; }
    [Required]
    public DateTime CreatedDate { get; set; }
    [Required]
    public DateTime UpdatedDate { get; set; }
    [Required]
    public int ProductId { get; set; }
    public Product Product { get; set; } // Navigation Property to Product
}
