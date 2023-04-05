using Mag.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using Mag.Auth;

namespace Mag.Controllers
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
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet("error/")]
        public IActionResult Error()
        {
            var resp = new Response { status = (int)TempData["status"], Message= TempData["Message"].ToString() };
            return View(resp);
        }
    }
}