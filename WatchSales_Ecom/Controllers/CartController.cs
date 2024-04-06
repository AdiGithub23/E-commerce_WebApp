using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WatchSales_Ecom.Services;

namespace WatchSales_Ecom.Controllers
{
    public class CartController : Controller
    {
        private readonly ShoppingCartService _shoppingCartService;

        public CartController(ShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> Index()
        {
            var cartItems = await _shoppingCartService.GetCartItems();
            return View(cartItems);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            await _shoppingCartService.RemoveFromCart(cartItemId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            await _shoppingCartService.UpdateCartItemQuantity(cartItemId, quantity);
            return RedirectToAction("Index");
        }

        // Implement additional methods for specific cart functionalities
    }
}
