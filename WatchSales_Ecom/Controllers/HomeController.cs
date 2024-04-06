using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
using WatchSales_Ecom.Data;
using WatchSales_Ecom.Models;
using WatchSales_Ecom.Models.DTO;

namespace WatchSales_Ecom.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Product.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());

        }
        #region Tryouts for SearchComponent
        //public async Task<IActionResult> Index(string sterm = "", int categoryId = 0)
        //{
        //    IEnumerable<Category> categories = await _context.Category.ToListAsync();

        //    sterm = sterm.ToLower();
        //    IEnumerable<Product> products = await (from prod in _context.Product
        //                                     join cat in _context.Category
        //                                     on prod.CategoryId equals cat.CategoryId
        //    where string.IsNullOrWhiteSpace(sterm) || (prod != null && prod.ProductName.ToLower().StartsWith(sterm))
        //    select new Product
        //    {
        //                                         ProductId = prod.ProductId,
        //                                         ProductImage = prod.ProductImage,
        //                                         ProductName = prod.ProductName, 
        //                                         ProductDescription = prod.ProductDescription,
        //                                         CategoryId = prod.CategoryId,
        //                                         ProductPrice = prod.ProductPrice,
        //                                         ProductQuantity = prod.ProductQuantity
        //    }).ToListAsync();

        //    if (categoryId > 0)
        //    {
        //        products = products.Where(a => a.CategoryId == categoryId).ToList();
        //    }

        //    ProductVM productVM = new ProductVM
        //    {
        //        Products = products,
        //        Categories = categories,
        //        STerm = sterm,
        //        CategoryId = categoryId
        //    };
        //    return View(productVM);

        //}


        //public async Task<IActionResult> Index(string searchString)
        //{
        //    if (_context.Product == null)
        //    {
        //        return Problem("Entity set 'course'  is null.");
        //    }

        //    var courses = from m in _context.Product
        //                  select m;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        courses = courses.Where(s => s.ProductName!.Contains(searchString));
        //    }
        //    courses = _context.Product.Include(p => p.Category);
        //    return View(courses);

        //}
        #endregion


        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
