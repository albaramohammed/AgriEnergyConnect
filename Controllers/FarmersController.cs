using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AgriEnergyConnect.Controllers
{
    [Authorize(Roles = "Employee")]
    public class FarmersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FarmersController> _logger;

        public FarmersController(ApplicationDbContext context, ILogger<FarmersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Farmers
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("FarmersController.Index() called");
            return View(await _context.Farmers.ToListAsync());
        }

        // GET: /Farmers/Create
        public IActionResult Create()
        {
            _logger.LogInformation("FarmersController.Create() called");
            return View();
        }

        // POST: /Farmers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email")] Farmer farmer)
        {
            _logger.LogInformation("FarmersController.Create(POST) called");

            if (ModelState.IsValid)
            {
                _context.Add(farmer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(farmer);
        }

        // GET: /Farmers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _logger.LogInformation($"FarmersController.Edit({id}) called");

            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers.FindAsync(id);
            if (farmer == null)
            {
                return NotFound();
            }
            return View(farmer);
        }

        // POST: /Farmers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FarmerId,Name,Email")] Farmer farmer)
        {
            _logger.LogInformation($"FarmersController.Edit({id}, POST) called");

            if (id != farmer.FarmerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(farmer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarmerExists(farmer.FarmerId))
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
            return View(farmer);
        }

        // GET: /Farmers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            _logger.LogInformation($"FarmersController.Delete({id}) called");

            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmers
                .FirstOrDefaultAsync(m => m.FarmerId == id);
            if (farmer == null)
            {
                return NotFound();
            }
            return View(farmer);
        }

        // POST: /Farmers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation($"FarmersController.DeleteConfirmed({id}) called");

            var farmer = await _context.Farmers.FindAsync(id);
            _context.Farmers.Remove(farmer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FarmerExists(int id)
        {
            return _context.Farmers.Any(e => e.FarmerId == id);
        }
    }
}