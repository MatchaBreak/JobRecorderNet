using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobRecorderNet.Models;

namespace JobRecorderNet.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Client
        public async Task<IActionResult> Index(string search, string column = "all")
        {
            if (!string.IsNullOrEmpty(search)) search = search.ToLower();

            var viewModel = new SearchBarViewModel
            {
                Title = "Clients",
                Search = search,
                PlaceHolder = "Search Clients...",
                IndexRoute = Url.Action("Index", "Client") ?? throw new InvalidOperationException(),
                CreateRoute = Url.Action("Create", "Client") ?? throw new InvalidOperationException(),
                Columns = new Dictionary<string, String>
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
                    {"postcode", "Postcode"}
                },
                SelectedColumn = column
            };

            var clientsQuery = _context.Clients.Include(c => c.Addresses).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                clientsQuery = column switch
                {
                    "all" => clientsQuery.Where(u => 
                        u.Name.ToLower().Contains(search) ||
                        u.Email.ToLower().Contains(search) ||
                        (u.Phone != null && u.Phone.ToLower().Contains(search)) ||
                        u.Mobile.ToLower().Contains(search) ||
                        // Multiple addresses
                        u.Addresses.Any(a => a.Name.ToLower().Contains(search) ||
                                             a.Street.ToLower().Contains(search) ||
                                             a.Suburb.ToLower().Contains(search) ||
                                             a.State.ToLower().Contains(search) ||
                                             a.Postcode.ToLower().Contains(search))),
                    "name" => clientsQuery.Where(c => c.Name.ToLower().Contains(search)),
                    "email" => clientsQuery.Where(c => c.Email.ToLower().Contains(search)),
                    "phone" => clientsQuery.Where(c => c.Phone != null && c.Phone.ToLower().Contains(search)),
                    "mobile" => clientsQuery.Where(c => c.Mobile.ToLower().Contains(search)),
                    "addressName" => clientsQuery.Where(c => c.Addresses.Any(a => a.Name.ToLower().Contains(search))),
                    "street" => clientsQuery.Where(c => c.Addresses.Any(a => a.Street.ToLower().Contains(search))),
                    "suburb" => clientsQuery.Where(c => c.Addresses.Any(a => a.Suburb.ToLower().Contains(search))),
                    "state" => clientsQuery.Where(c => c.Addresses.Any(a => a.State.ToLower().Contains(search))),
                    "postcode" => clientsQuery.Where(c => c.Addresses.Any(a => a.Postcode.ToLower().Contains(search))),
                    _ => clientsQuery
                };
            }

            // list of clients returned in the view
            var clients = await clientsQuery.ToListAsync();
            int clientCount = await _context.Clients.CountAsync();
            ViewBag.ClientCount = clientCount;

            ViewData["SearchBarViewModel"] = viewModel;
            return View(clients);
        }

        // GET: Client/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Client/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client, Address address)
        {
            if (ModelState.IsValid)
            {
                client.CreatedAt = DateTime.Now;
                client.UpdatedAt = DateTime.Now;

                if(address != null) 
                {
                    address.CreatedAt = DateTime.Now;
                    address.UpdatedAt = DateTime.Now;
                    client.Addresses.Add(address);
                }
                // Saves the Client and Addresses together
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Client/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Phone,Mobile,CreatedAt,UpdatedAt")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(client);
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
