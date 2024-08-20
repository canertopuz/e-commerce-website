using Microsoft.AspNetCore.Identity;
using UyumsoftProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
public class CartService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<Cart> GetCartAsync(string userId)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        return cart;
    }

    public async Task AddToCartAsync(string userId, string productName, int productId, decimal price, int quantity)
    {
        var cart = await GetCartAsync(userId);
        var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

        if (cartItem == null)
        {
            cartItem = new CartItem
            {
                ProductName = productName,
                ProductId = productId,
                Price = price,
                Quantity = quantity,
                CartId = cart.Id
            };
            cart.Items.Add(cartItem);
        }
        else
        {
            cartItem.Quantity += quantity;
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveFromCartAsync(string userId, int itemId)
    {
        var cart = await GetCartAsync(userId);
        var cartItem = cart.Items.FirstOrDefault(i => i.Id == itemId);

        if (cartItem != null)
        {
            cart.Items.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(string userId)
    {
        var cart = await GetCartAsync(userId);
        _context.CartItems.RemoveRange(cart.Items);
        await _context.SaveChangesAsync();
    }
}
