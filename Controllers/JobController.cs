using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobRecorderNet.Models;

namespace JobRecorderNet.Controllers
{
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobController(ApplicationDbContext context)
        {
            _context = context;
        }

        protected string GenerateJobNumber()
        {
            //use CreatedAt
            int year = DateTime.Now.Year % 100;
            int quarter = (DateTime.Now.Month - 1) / 3 + 1;
            int jobCount = _context.Jobs.Count(j => j.CreatedAt.Year == year && (j.CreatedAt.Month - 1) / 3 + 1 == quarter);
            jobCount++;
            return $"J{year}{quarter}{jobCount:D3}";
        }

        // GET: Job
        public async Task<IActionResult> Index(string search, string column = "all")
        {
            if (!string.IsNullOrEmpty(search)) search = search.ToLower();

            var viewModel = new SearchBarViewModel
            {
                Title = "Jobs",
                Search = search,
                PlaceHolder = "Search Jobs...",
                IndexRoute = Url.Action("Index", "Job") ?? throw new InvalidOperationException(),
                CreateRoute = Url.Action("Create", "Job") ?? throw new InvalidOperationException(),
                Columns = new Dictionary<string, string>
                {
                    {"all", "All"}, // Add "all" as a filter option
                    {"jobNo", "Job No"},
                    {"type", "Type"},
                    {"clientName", "Client Name"},
                    {"supervisorName", "Supervisor Name"},
                    {"addressName", "Address Name"},
                    {"street", "Street"},
                    {"suburb", "Suburb"},
                    {"state", "State"},
                    {"postcode", "Postcode"},
                    {"description", "Description"},
                    {"status", "Status"}
                },
                SelectedColumn = column
            };

            var jobsQuery = _context.Jobs
                .Include(j => j.Address)
                .Include(j => j.Client)
                .Include(j => j.Supervisor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                jobsQuery = column switch
                {
                    "all" => jobsQuery.Where(j =>
                        j.JobNo.ToLower().Contains(search) ||
                        j.Type.ToString().ToLower().Contains(search) ||
                        j.Client.Name.ToLower().Contains(search) ||
                        j.Supervisor.Name.ToLower().Contains(search) ||
                        (j.Address != null && (j.Address.Name.ToLower().Contains(search) ||
                                               j.Address.Street.ToLower().Contains(search) ||
                                               j.Address.Suburb.ToLower().Contains(search) ||
                                               j.Address.State.ToLower().Contains(search) ||
                                               j.Address.Postcode.ToLower().Contains(search))) ||
                        (j.Description != null && j.Description.ToLower().Contains(search)) ||
                        j.Status.ToString().ToLower().Contains(search)),
                    "jobNo" => jobsQuery.Where(j => j.JobNo.ToLower().Contains(search)),
                    "type" => jobsQuery.Where(j => j.Type.ToString().ToLower().Contains(search)),
                    "clientName" => jobsQuery.Where(j => j.Client.Name.ToLower().Contains(search)),
                    "supervisorName" => jobsQuery.Where(j => j.Supervisor.Name.ToLower().Contains(search)),
                    "addressName" => jobsQuery.Where(j => j.Address.Name.ToLower().Contains(search)),
                    "street" => jobsQuery.Where(j => j.Address.Street.ToLower().Contains(search)),
                    "suburb" => jobsQuery.Where(j => j.Address.Suburb.ToLower().Contains(search)),
                    "state" => jobsQuery.Where(j => j.Address.State.ToLower().Contains(search)),
                    "postcode" => jobsQuery.Where(j => j.Address.Postcode.ToLower().Contains(search)),
                    "description" => jobsQuery.Where(j => (j.Description != null && j.Description.ToLower().Contains(search))),
                    "status" => jobsQuery.Where(j => j.Status.ToString().ToLower().Contains(search)),
                    _ => jobsQuery
                };
            }

            var jobs = await jobsQuery.ToListAsync();

            ViewData["SearchBarViewModel"] = viewModel;
            return View(jobs);

        }

        // GET: Job/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Address)
                .Include(j => j.Client)
                .Include(j => j.Supervisor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Job/Create
        public IActionResult Create()
        {
            // Fetch clients and their addresses
            var clients = _context.Clients
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    Addresses = _context.Addresses
                        .Where(a => a.ClientId == c.Id)
                        .Select(a => new
                        {
                            a.Id,
                            a.Name,
                            Label = $"{a.Name}: {a.Street}, {a.Suburb}, {a.State} {a.Postcode}"
                        })
                        .ToList()
                })
                .ToList();
            ViewBag.Clients = clients;
            ViewBag.ClientId = new SelectList(clients, "Id", "Name");
            // For addresses
            ViewBag.AddressId = new List<SelectListItem>();

            //Supervisor 
            ViewBag.SupervisorId = new SelectList(_context.Users, "Id", "Name");

            //technicians 
            ViewBag.Technicians = _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name
                })
                .ToList(); 
                

            // Populates JobType dropdown using enum in Job model file
            ViewBag.JobType = Enum.GetValues(typeof(JobType))
                .Cast<JobType>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                })
                .ToList();

            // Prepopulate the JobNo field with a generated job number
            ViewBag.JobNumber = GenerateJobNumber();
            return View();
        }

        // POST: Job/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> Create([Bind("Id,JobNo,Type,ClientId,SupervisorId,AddressId,Description,Status,CreatedAt,UpdatedAt")] Job job)
        {
            if (ModelState.IsValid)
            {
                job.JobNo = GenerateJobNumber();
                job.CreatedAt = DateTime.Now;

                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if model is invalid
            var clients = _context.Clients
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    Addresses = _context.Addresses
                        .Where(a => a.ClientId == c.Id)
                        .Select(a => new
                        {
                            a.Id,
                            a.Name,
                            Label = $"{a.Name}: {a.Street}, {a.Suburb}, {a.State} {a.Postcode}"
                        })
                        .ToList()
                })
                .ToList();

            ViewBag.Clients = clients;
            ViewBag.ClientId = new SelectList(clients, "Id", "Name", job.ClientId);

            ViewBag.AddressId = _context.Addresses
                .Where(a => a.ClientId == job.ClientId) // show only relevant addresses
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Name}: {a.Street}, {a.Suburb}, {a.State} {a.Postcode}"
                })
                .ToList();

            ViewBag.SupervisorId = new SelectList(_context.Users, "Id", "Name", job.SupervisorId);

            ViewBag.Technicians = _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name
                })
                .ToList();

            ViewBag.JobType = Enum.GetValues(typeof(JobType))
                .Cast<JobType>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                })
                .ToList();

            ViewBag.JobNumber = job.JobNo; // Keep generated number visible

            return View(job);
        }

        // GET: Job/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Address)
                .Include(j => j.Client)
                .Include(j => j.Supervisor)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            // Populate ViewBag for dropdowns
            var clients = _context.Clients
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    Addresses = _context.Addresses
                        .Where(a => a.ClientId == c.Id)
                        .Select(a => new
                        {
                            a.Id,
                            a.Name,
                            Label = $"{a.Name}: {a.Street}, {a.Suburb}, {a.State} {a.Postcode}"
                        })
                        .ToList()
                })
                .ToList();

            ViewBag.Clients = clients;
            ViewBag.ClientId = new SelectList(clients, "Id", "Name", job.ClientId);

            ViewBag.AddressId = _context.Addresses
                .Where(a => a.ClientId == job.ClientId) // Show only relevant addresses
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Name}: {a.Street}, {a.Suburb}, {a.State} {a.Postcode}"
                })
                .ToList();

            ViewBag.SupervisorId = new SelectList(_context.Users, "Id", "Name", job.SupervisorId);

            ViewBag.Technicians = _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name
                })
                .ToList();

            ViewBag.JobType = Enum.GetValues<JobType>()
                .Cast<JobType>()    // Projects eeach num value into a SelectListItem object
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                })
                .ToList();

            return View(job);
        }

        // POST: Job/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JobNo,Type,ClientId,SupervisorId,AddressId,Description,Status,CreatedAt,UpdatedAt")] Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update timestamp
                    job.UpdatedAt = DateTime.Now;

                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdowns if ModelState is invalid
            var clients = _context.Clients
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    Addresses = _context.Addresses
                        .Where(a => a.ClientId == c.Id)
                        .Select(a => new
                        {
                            a.Id,
                            a.Name,
                            Label = $"{a.Name}: {a.Street}, {a.Suburb}, {a.State} {a.Postcode}"
                        })
                        .ToList()
                })
                .ToList();

            ViewBag.Clients = clients;
            ViewBag.ClientId = new SelectList(clients, "Id", "Name", job.ClientId);

            ViewBag.AddressId = _context.Addresses
                .Where(a => a.ClientId == job.ClientId) // Show only relevant addresses
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Name}: {a.Street}, {a.Suburb}, {a.State} {a.Postcode}"
                })
                .ToList();

            ViewBag.SupervisorId = new SelectList(_context.Users, "Id", "Name", job.SupervisorId);

            ViewBag.Technicians = _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name
                })
                .ToList();

            ViewBag.JobType = Enum.GetValues<JobType>()
                .Cast<JobType>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                })
                .ToList();

            return View(job);
        }

        // GET: Job/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .Include(j => j.Address)
                .Include(j => j.Client)
                .Include(j => j.Supervisor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }
    }
}
