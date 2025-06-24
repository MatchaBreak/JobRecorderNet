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
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User, also handles search + adding user button
       public async Task<IActionResult> Index(string search, string column = "all") // Default to "all"
        {
            search = search?.ToLower();

            var viewModel = new SearchBarViewModel
            {
                Title = "Users",
                Search = search,
                PlaceHolder = "Search Users...",
                IndexRoute = Url.Action("Index", "User"),
                CreateRoute = Url.Action("Create", "User"),
                Columns = new Dictionary<string, string>
                {
                    {"all", "All"}, // Add "all" as a filter option
                    {"name", "Name"},
                    {"email", "Email"},
                    {"phone", "Phone"},
                    {"mobile", "Mobile"},
                    {"addressName", "Address Name"},
                    {"street", "Street"},
                    {"suburb", "Suburb"},
                    {"state", "State"},
                    {"postcode", "Postcode"},
                    {"role", "Role"}
                },
                SelectedColumn = column
            };

            var usersQuery = _context.Users.Include(u => u.Address).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                usersQuery = column switch
                {
                    "all" => usersQuery.Where(u =>
                        u.Name.ToLower().Contains(search) ||
                        u.Email.ToLower().Contains(search) ||
                        (u.Phone != null && u.Phone.ToLower().Contains(search)) ||
                        u.Mobile.ToLower().Contains(search) ||
                        u.Address.Name.ToLower().Contains(search) ||
                        u.Address.Street.ToLower().Contains(search) ||
                        u.Address.Suburb.ToLower().Contains(search) ||
                        u.Address.State.ToLower().Contains(search) ||
                        u.Address.Postcode.ToLower().Contains(search) ||
                        u.Role.ToString().ToLower().Contains(search)),
                    "name" => usersQuery.Where(u => u.Name.ToLower().Contains(search)),
                    "email" => usersQuery.Where(u => u.Email.ToLower().Contains(search)),
                    "phone" => usersQuery.Where(u => u.Phone != null && u.Phone.ToLower().Contains(search)),
                    "mobile" => usersQuery.Where(u => u.Mobile.ToLower().Contains(search)),
                    "addressName" => usersQuery.Where(u => u.Address.Name.ToLower().Contains(search)),
                    "street" => usersQuery.Where(u => u.Address.Street.ToLower().Contains(search)),
                    "suburb" => usersQuery.Where(u => u.Address.Suburb.ToLower().Contains(search)),
                    "state" => usersQuery.Where(u => u.Address.State.ToLower().Contains(search)),
                    "postcode" => usersQuery.Where(u => u.Address.Postcode.ToLower().Contains(search)),
                    "role" => usersQuery.Where(u => u.Role.ToString().ToLower().Contains(search)),
                    _ => usersQuery
                };
            }

            var users = await usersQuery.ToListAsync();

            ViewData["SearchBarViewModel"] = viewModel;
            return View(users);
        }
        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                // Saves the User and Address together
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }


        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,Phone,Mobile,Address,Role,CreatedAt,UpdatedAt")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
