using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UyumsoftProject.Models;

[Authorize]
public class CartController : Controller
{
    private readonly CartService _cartService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public CartController(CartService cartService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _cartService = cartService;
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User); ; // Kullanıcı ID'sini al
        var cart = _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.UserId == userId);
        var addresses = _context.Address.Where(a => a.UserId == userId).ToList(); // Kullanıcıya ait adresleri al

        var model = new CartViewModel
        {
            Cart = cart,
            Addresses = addresses
        };

        return View(model);

    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(string productName, int productId, decimal price, int quantity)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }
        await _cartService.AddToCartAsync(userId, productName, productId, price, quantity);
        return RedirectToAction("CartPartial");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int itemId)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }
        await _cartService.RemoveFromCartAsync(userId, itemId);
        return RedirectToAction("Index", "Cart");
    }

    [HttpPost]
    public async Task<IActionResult> ClearCart()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }
        await _cartService.ClearCartAsync(userId);
        return RedirectToAction("Index", "Cart");
    }

    public async Task<IActionResult> CartPartial()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }
        var cart = await _cartService.GetCartAsync(userId);
        var cartItems = cart?.Items ?? new List<CartItem>();

        return PartialView("_CartPartial", cartItems); // Partial view'ı döner
    }
}
