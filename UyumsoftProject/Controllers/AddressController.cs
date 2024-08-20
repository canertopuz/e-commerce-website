using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using UyumsoftProject.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UyumsoftProject.Controllers
{
    public class AddressController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddressController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Address
        //public async Task<IActionResult> Index()
        //{
        //    var addresses = await _context.Address.ToListAsync();
        //    return View(addresses);
        //}
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var addresses = await _context.Address
                                          .Where(a => a.UserId == user.Id)
                                          .ToListAsync();
            return View(addresses);
        }

        // GET: Address/Create
        public IActionResult Create()
        {
            ViewData["Cities"] = new SelectList(_context.City, "Id", "Name");
            return View();
        }

        // POST: Address/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("CityId,Name,AddressDetail,PostCode,UserId")] Address address)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(address);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["Cities"] = new SelectList(_context.City, "Id", "Name", address.CityId);
        //    return View(address);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CityId,Name,AddressDetail,PostCode,UserId")] Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Cities"] = new SelectList(_context.City, "Id", "Name", address.CityId);
            return View(address);
        }


        // GET: Address/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            ViewData["Cities"] = new SelectList(_context.City, "Id", "Name", address.CityId);
            return View(address);
        }

        // POST: Address/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CityId,Name,AddressDetail,PostCode,UserId")] Address address)
        {
            if (id != address.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Cities"] = new SelectList(_context.City, "Id", "Name", address.CityId);
            return View(address);
        }

        // GET: Address/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .FirstOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Address/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _context.Address.FindAsync(id);
            _context.Address.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(int id)
        {
            return _context.Address.Any(e => e.Id == id);
        }
    }
}
