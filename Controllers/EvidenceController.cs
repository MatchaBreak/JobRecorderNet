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
    public class EvidenceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EvidenceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Evidence
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Evidence.Include(e => e.Job);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Evidence/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evidence = await _context.Evidence
                .Include(e => e.Job)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evidence == null)
            {
                return NotFound();
            }

            return View(evidence);
        }

        // GET: Evidence/Create
        public IActionResult Create()
        {
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "JobNo");
            return View();
        }

        // POST: Evidence/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JobId,Title,Type,Path,Description,CreatedAt,UpdatedAt")] Evidence evidence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evidence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "JobNo", evidence.JobId);
            return View(evidence);
        }

        // GET: Evidence/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evidence = await _context.Evidence.FindAsync(id);
            if (evidence == null)
            {
                return NotFound();
            }
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "JobNo", evidence.JobId);
            return View(evidence);
        }

        // POST: Evidence/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JobId,Title,Type,Path,Description,CreatedAt,UpdatedAt")] Evidence evidence)
        {
            if (id != evidence.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evidence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvidenceExists(evidence.Id))
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
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "JobNo", evidence.JobId);
            return View(evidence);
        }

        // GET: Evidence/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evidence = await _context.Evidence
                .Include(e => e.Job)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evidence == null)
            {
                return NotFound();
            }

            return View(evidence);
        }

        // POST: Evidence/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evidence = await _context.Evidence.FindAsync(id);
            if (evidence != null)
            {
                _context.Evidence.Remove(evidence);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvidenceExists(int id)
        {
            return _context.Evidence.Any(e => e.Id == id);
        }
    }
}
