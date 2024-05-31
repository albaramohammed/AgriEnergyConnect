using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnect.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, "Farmer"))
            {
                var farmer = await _context.Farmers
                                           .Include(f => f.Products)
                                           .FirstOrDefaultAsync(f => f.Email == user.Email);
                return View(farmer.Products.ToList());
            }
            return View(await _context.Products.ToListAsync());
        }

        [Authorize(Roles = "Farmer")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Category,ProductionDate,FarmerId")] Product product)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.Email == user.Email);
                product.FarmerId = farmer.FarmerId;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
    }
}