using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UyumsoftProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace UyumsoftProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index(string searchTerm, List<int> categoryIds, List<int> brandIds)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.Name.Contains(searchTerm));
            }

            if (categoryIds != null && categoryIds.Any())
            {
                products = products.Where(p => categoryIds.Contains(p.CategoryId));
            }

            if (brandIds != null && brandIds.Any())
            {
                products = products.Where(p => brandIds.Contains(p.BrandId));
            }

            var model = new ProductViewModel
            {
                Products = products.ToList(),
                Categories = _context.Categories.ToList(),
                Brands = _context.Brands.ToList(),
                SelectedCategoryIds = categoryIds ?? new List<int>(),
                SelectedBrandIds = brandIds ?? new List<int>(),
                SearchTerm = searchTerm
            };

            return View(model);
        }
        public IActionResult Privacy()
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
