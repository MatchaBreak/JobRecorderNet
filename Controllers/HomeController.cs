using System.Diagnostics;
using JobRecorderNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobRecorderNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult Clients()
        {
            return View();
        }
        public IActionResult Addresses()
        {
            return View();
        }

        public IActionResult Jobs()
        {
            return View();
        }

        public IActionResult MyJobs()
        {
            return View();
        }

        public IActionResult TRCs()
        {
            return View();
        }

        public IActionResult MyTRCs()
        {
            return View();
        }

        public IActionResult Licences()
        {
            return View();
        }
        public IActionResult MyLicences()
        {
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
