using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobRecorderNet.Models;

namespace JobRecorderNet.Controllers
{
    public class LicenseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LicenseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: License
        public async Task<IActionResult> Index()
        {
            return View(await _context.Licenses.ToListAsync());
        }

        // GET: License/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var license = await _context.Licenses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (license == null)
            {
                return NotFound();
            }

            return View(license);
        }

        // GET: License/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: License/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Expiry,CreatedAt,UpdatedAt")] License license)
        {
            if (ModelState.IsValid)
            {
                _context.Add(license);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(license);
        }

        // GET: License/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var license = await _context.Licenses.FindAsync(id);
            if (license == null)
            {
                return NotFound();
            }
            return View(license);
        }

        // POST: License/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Expiry,CreatedAt,UpdatedAt")] License license)
        {
            if (id != license.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(license);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LicenseExists(license.Id))
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
            return View(license);
        }

        // GET: License/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var license = await _context.Licenses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (license == null)
            {
                return NotFound();
            }

            return View(license);
        }

        // POST: License/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var license = await _context.Licenses.FindAsync(id);
            if (license != null)
            {
                _context.Licenses.Remove(license);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LicenseExists(int id)
        {
            return _context.Licenses.Any(e => e.Id == id);
        }
    }
}
