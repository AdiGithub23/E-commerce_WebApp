using System.ComponentModel.DataAnnotations;

namespace WatchSales_Ecom.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public int ProductId { get; set; } 
        public int Quantity { get; set; }
        public string? UserId { get; set; } 
        public Product? Product { get; set; } // Navigation property
    }
}
