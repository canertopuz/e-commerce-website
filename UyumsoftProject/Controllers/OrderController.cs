using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UyumsoftProject.Models;
using System.Threading.Tasks;
using System.Linq;

[Authorize]
public class OrderController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public OrderController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var orders = await _context.Orders
                                   .Include(o => o.OrderItems)
                                   .Where(o => o.UserId == userId)
                                   .ToListAsync();
        return View(orders);
    }
    [Authorize(Roles="Admin,Manager")]
    public async Task<IActionResult> AllOrders()
    {
        var orders = await _context.Orders
                                   .Include(o => o.OrderItems)
                                   .Include(o => o.Address)
                                   .ToListAsync();

        var totalRevenue = orders.Sum(o => o.OrderItems.Sum(oi => oi.Quantity * oi.Price));

        var viewModel = new AllOrdersViewModel
        {
            Orders = orders,
            TotalRevenue = (decimal)totalRevenue
        };

        return View(viewModel);
    }
    public async Task<IActionResult> Details(int id)
    {
        var userId = _userManager.GetUserId(User);
        var order = await _context.Orders
                                  .Include(o => o.OrderItems)
                                  .Include(o => o.Address) 
                                  .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(int addressId)
    {
        var userId = _userManager.GetUserId(User);
        var cart = await _context.Carts
                                 .Include(c => c.Items)
                                 .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.Items.Any())
        {
            return BadRequest("Sepetiniz boş.");
        }

        var order = new Order
        {
            UserId = userId,
            AddressId = addressId,
            OrderDate = DateTime.Now,
            OrderItems = cart.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Price = (double)i.Price,
                Quantity = i.Quantity
            }).ToList()
        };



        _context.Orders.Add(order);

        // Stok güncelleme işlemi
        foreach (var item in cart.Items)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
            if (product != null)
            {
                product.Stock -= item.Quantity;
                if (product.Stock < 0)
                {
                    return BadRequest($"Ürün {product.Name} için yeterli stok yok.");
                }
            }
        }

        _context.Carts.Remove(cart);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}