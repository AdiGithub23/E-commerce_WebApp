using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WatchSales_Ecom.Data;
using WatchSales_Ecom.Models;
using WatchSales_Ecom.Models.DTO;
using WatchSales_Ecom.Services;

namespace WatchSales_Ecom.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ApplicationDbContext context, ShoppingCartService shoppingCartService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _shoppingCartService = shoppingCartService;
            _webHostEnvironment = webHostEnvironment;
        }

        //[HttpPost]
        [Authorize(Policy = "AdminOnly")]
        // GET: Products
        public async Task<IActionResult> Index()
        {
            if (_context.Product== null)
            {
                return Problem("Entity set is null.");
            }

            var applicationDbContext = _context.Product.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());

        }


        [Authorize(Policy = "AdminOnly")]
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        [Authorize(Policy = "AdminOnly")]
        // GET: Products/Create
        public IActionResult Create()
        {
            //ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryId");
            ViewData["Categoryies"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryName");
            return View();
        }

        [Authorize(Policy = "AdminOnly")]
        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,CategoryId,ProductPrice,ProductQuantity,ImageFile,ProductDescription")] ProductVM productDTO)
        {

            if (productDTO.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(productDTO);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDTO.ImageFile!.FileName);
            string imageFullPath = _webHostEnvironment.WebRootPath + "/images/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDTO.ImageFile.CopyTo(stream);
            }

            Product product = new Product()
            {
                ProductName = productDTO.ProductName,
                ProductPrice = productDTO.ProductPrice,
                ProductDescription = productDTO.ProductDescription,
                ProductQuantity = productDTO.ProductQuantity,

                CategoryId = productDTO.CategoryId,

                ProductImage = newFileName
            };
            //if (product.CategoryId == 0)
            //{
            //    ModelState.AddModelError("CategoryId", "Please select a category.");
            //    return View(productDTO);
            //}
                        
            _context.Product.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index", "Products");


        }

        #region Original Create
        //public async Task<IActionResult> Create([Bind("ProductId,ProductName,CategoryId,ProductPrice,ProductQuantity,ProductImage,ProductDescription")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryId", product.CategoryId);
        //    return View(product);

        //}
        #endregion


        #region Image Upload Tryouts
        //public async Task<IActionResult> Create([Bind("ProductId,ProductName,CategoryId,ProductPrice,ProductQuantity,ProductImage,ProductDescription")] Product product, IFormFile Image)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (Image != null && Image.Length > 0)
        //        {
        //            // Generate unique filename
        //            var fileName = Path.GetFileNameWithoutExtension(Image.FileName)
        //                + DateTime.Now.ToString("yyyyMMddHHmmss")
        //                + Path.GetExtension(Image.FileName);

        //            // Upload the image to wwwroot/images (or desired folder)
        //            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await Image.CopyToAsync(stream);
        //            }

        //            product.ProductImage = fileName; // Set the image filename in the model
        //        }
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryId", product.CategoryId);
        //    return View(product);
        //}

        #endregion


        [Authorize(Policy = "AdminOnly")]
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var product = await _context.Product.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }
            if (product == null)
            {
                return NotFound();
            }

            ///////////////////////////////////////////////////////////////////////////////
            var productDTO = new ProductVM()
            {
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductQuantity = product.ProductQuantity,
                ProductDescription = product.ProductDescription,
                CategoryId = product.CategoryId

            };

            ViewData["ProductImage"] = product.ProductImage;
            ViewData["Categoryies"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryName", product.CategoryId);

            return View(productDTO);
            ///////////////////////////////////////////////////////////////////////////////

            //ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryId", product.CategoryId);
            //return View(product);
        }

        [Authorize(Policy = "AdminOnly")]
        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,CategoryId,ProductPrice,ProductQuantity,ImageFile,ProductDescription")] ProductVM productDTO)
        {
            Product? product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                ViewData["ProductImage"] = product.ProductImage;
                return View(productDTO);
            }

            string? newFileName = product.ProductImage;
            if (productDTO.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDTO.ImageFile!.FileName);
                string imageFullPath = _webHostEnvironment.WebRootPath + "/images/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDTO.ImageFile.CopyTo(stream);
                }

                // delete old image
                string oldImageFullPath = _webHostEnvironment.WebRootPath + "/images/" + product.ProductImage;
                System.IO.File.Delete(oldImageFullPath);
            }

            product.ProductName = productDTO.ProductName;
            product.ProductPrice = productDTO.ProductPrice;
            product.ProductQuantity = productDTO.ProductQuantity;
            product.ProductDescription = productDTO.ProductDescription;
            product.CategoryId = productDTO.CategoryId;

            product.ProductImage = newFileName;

            _context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }

        #region Original Edit
        //public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,CategoryId,ProductPrice,ProductQuantity,ProductImage,ProductDescription")] Product product)
        //{
        //    if (id != product.ProductId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(product);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductExists(product.ProductId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryId", product.CategoryId);
        //    return View(product);
        //}
        #endregion


        [Authorize(Policy = "AdminOnly")]
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Policy = "AdminOnly")]
        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }




        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {

            await _shoppingCartService.AddToCart(productId, quantity);

            // Redirect to a specific cart view or the product details page with a success message
            return RedirectToAction("Index", "Home"); // Example redirect , new { id = productId }
        }
    }
}
