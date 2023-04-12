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
        [HttpGet("error/{statusCode}")]
        public IActionResult Error(int? statusCode)
        {
            if(statusCode == null) 
            { 
                var resp = new Response { Status = (int)TempData["Status"], Message= TempData["Message"].ToString() };
                return View(resp);
            }
            return View(new Response {Status = statusCode, Message = "Not found"});
        }
    }
}