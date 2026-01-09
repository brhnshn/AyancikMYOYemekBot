using AyancikYemekWeb.Models;
using AyancikYemekWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AyancikYemekWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ScraperService _scraperService; 

        // Servisi burada içeri alýyoruz (Dependency Injection)
        public HomeController(ILogger<HomeController> logger, ScraperService scraperService)
        {
            _logger = logger;
            _scraperService = scraperService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _scraperService.MenuyuGetirAsync();

            return View(model);
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