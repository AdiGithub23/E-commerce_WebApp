using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchSales_Ecom.Models
{
    [Table("Product")]
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        [Required]
        public string? ProductDescription { get; set; }


        //[Required]
        public string? ProductImage { get; set; }
        
        public Category? Category { get; set; } // Navigation property
    }
}
