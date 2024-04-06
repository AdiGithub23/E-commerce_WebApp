using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


// This class is created for the functionality of Search Component

namespace WatchSales_Ecom.Models.DTO
{
    public class ProductVM
    {
        //public IEnumerable<Product> Products { get; set; } // Navigation property
        //public IEnumerable<Category> Categories { get; set; } // Navigation property
        //public string STerm { get; set; } = "";
        //public int CategoryId { get; set; } = 0;

        public int ProductId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        [Required]
        public string? ProductDescription { get; set; }
        public Category? Category { get; set; } // Navigation property

        public IFormFile? ImageFile { get; set; }

    }
}
