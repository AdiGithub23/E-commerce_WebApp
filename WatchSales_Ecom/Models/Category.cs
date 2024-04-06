using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchSales_Ecom.Models
{
    [Table("Category")]
    public class Category
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string? CategoryName { get; set; }
        //[ValidateNever]
        public IEnumerable<Product>? Products { get; set; } // Navigation property
    }
}
