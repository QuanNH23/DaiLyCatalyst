using HealMe.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HealMe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Feedback()
        {
            return View("~/Views/FeedBacks/HealMeFeedbackVer2.cshtml");
        }
        [HttpGet]
        public IActionResult BuyNow()
        {
            return View("~/Views/BuyPageHealMe.cshtml");
        }

        public IActionResult Index()
        {
            return View("~/Views/Home/HomePage.cshtml");
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
