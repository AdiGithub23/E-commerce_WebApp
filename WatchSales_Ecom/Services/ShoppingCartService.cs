using Microsoft.EntityFrameworkCore;
using WatchSales_Ecom.Data;
using WatchSales_Ecom.Models;

namespace WatchSales_Ecom.Services
{
    public class ShoppingCartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            string userId = _httpContextAccessor.HttpContext?.User.Identity.Name; // Get logged-in user ID (if using UserId)
            var cartItems = await _context.CartItems
                .Include(item => item.Product)
                .Where(item => item.UserId == userId) // Filter by user if using UserId
                .ToListAsync();
            return cartItems;
        }

        public async Task AddToCart(int productId, int quantity)
        {
            string? userId = _httpContextAccessor.HttpContext?.User.Identity?.Name; // Get logged-in user ID (if using UserId)

            // Check if item already exists in cart
            var existingCartItem = await _context.CartItems
                .Where(item => item.ProductId == productId && item.UserId == userId) // Filter by user if using UserId
                .FirstOrDefaultAsync();

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    UserId = userId, // Set if using UserId
                    Quantity = quantity
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemQuantity(int cartItemId, int quantity)
        {

            var cart = await _context.CartItems.FindAsync(cartItemId);
            if (cart != null)
            {
                if (quantity <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be > zero.");
                }

                cart.Quantity = quantity;
                _context.CartItems.Update(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromCart(int cartItemId)
        {

            var cart = await _context.CartItems.FindAsync(cartItemId);
            if (cart != null)
            {
                _context.CartItems.Remove(cart);
            }
            await _context.SaveChangesAsync();
        
        }



    }
}
