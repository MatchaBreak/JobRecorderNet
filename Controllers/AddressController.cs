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
    public class AddressController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AddressController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Address
        public async Task<IActionResult> Index(string search, string column = "isMain")
        {
             if (!string.IsNullOrEmpty(search)) search = search.ToLower();

            var viewModel = new SearchBarViewModel
            {
                Title = "Addresses",
                Search = search,
                PlaceHolder = "Search Addresses...",
                IndexRoute = Url.Action("Index", "Address"),
                CreateRoute = Url.Action("Create", "Address"),
                Columns = new Dictionary<string, string>
                {
                    {"all", "All"},
                    {"addressName", "Address Name"},
                    {"street", "Street"},
                    {"suburb", "Suburb"},
                    {"state", "State"},
                    {"postcode", "Postcode"},
                    {"IsMain", "Is Main"}
                },
                SelectedColumn = column

            };

            var usersQuery = _context.Addresses.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                usersQuery = column switch
                {
                    "all" => usersQuery,
                    "addressName" => usersQuery.Where(a => a.Name.ToLower().Contains(search)),
                    "street" => usersQuery.Where(a => a.Street.ToLower().Contains(search)),
                    "suburb" => usersQuery.Where(a => a.Suburb.ToLower().Contains(search)),
                    "state" => usersQuery.Where(a => a.State.ToLower().Contains(search)),
                    "postcode" => usersQuery.Where(a => a.Postcode.ToLower().Contains(search)),
                    "IsMain" => usersQuery.Where(a => a.IsMain),
                    _ => usersQuery.Where(a=>a.IsMain)
                };
            }
            var addresses = await usersQuery.ToListAsync();

            ViewData["SearchBarViewModel"] = viewModel;
            return View(addresses);
        }

        // GET: Address/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Address/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Address/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Street,Suburb,State,Postcode,IsMain,CreatedAt,UpdatedAt")] Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }

        // GET: Address/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        // POST: Address/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Street,Suburb,State,Postcode,IsMain,CreatedAt,UpdatedAt")] Address address)
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
            return View(address);
        }

        // GET: Address/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
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
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}
