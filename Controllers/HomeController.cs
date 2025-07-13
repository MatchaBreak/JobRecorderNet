using System.Diagnostics;
using JobRecorderNet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobRecorderNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        [Authorize]
         public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                ViewBag.Name = user.Name;
                ViewBag.Role = user.Role.ToString();
            }
            else
            {
                ViewBag.Name = "Guest";
                ViewBag.Role = "Viewer";
            }

            int userCount = await _context.Users.CountAsync();
            int clientCount = await _context.Clients.CountAsync();
            int jobCount = await _context.Jobs.CountAsync();

            ViewBag.UserCount = userCount;
            ViewBag.ClientCount = clientCount;
            ViewBag.JobCount = jobCount;

            return View();
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
