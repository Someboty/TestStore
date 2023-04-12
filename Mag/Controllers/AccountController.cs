using Mag.Interfaces;
using Mag.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Mag.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IUserService _userService;
        private readonly ApplicationContext _context;

        public AccountController(UserManager<AspNetUser> userManager, IUserService userService, ApplicationContext context)
        {
            _userManager = userManager;
            _userService = userService;
            _context = context;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Status"] = 400;
                TempData["Message"] = "Invalid requst";
                return RedirectToAction("Error", "Home");
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["Status"] = 401;
                TempData["Message"] = "Email or password is invalid";
                return RedirectToAction("Error", "Home");
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
            {
                TempData["Status"] = 401;
                TempData["Message"] = "Email or password is invalid";
                return RedirectToAction("Error", "Home");
            }
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user!.Id),
                    new Claim(ClaimTypes.Name, user!.UserName!),
                    new Claim(ClaimTypes.Email, user!.Email!),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var identity = new ClaimsIdentity(authClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var authProps = new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                RedirectUri = "/"
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);
            return Redirect("/");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByNameAsync(model.Email);
                if (userExists != null)
                {
                    TempData["Status"] = 400;
                    TempData["Message"] = "User already exists!";
                    return RedirectToAction("Error", "Home");
                }

                AspNetUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                    TempData["Status"] = 400;
                    TempData["Message"] = $"User creation failed! {errorMessage}";
                    return RedirectToAction("Error", "Home");
                }
                return RedirectToAction("Login");
            }
            TempData["Status"] = 400;
            TempData["Message"] = "Invalid request";
            return RedirectToAction("Error", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            ViewBag.NavBar = 1;
            return View(await _userService.CurrentUser());
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(AspNetUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.CurrentUser()!;
                var profile = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
                profile.FirstName = model.FirstName;
                profile.LastName = model.LastName;
                profile.SecondName = model.SecondName;
                profile.Gender = model.Gender;
                profile.DOB = model.DOB;
                profile.Email = model.Email;
                _context.Users.Update(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction("Profile");
            }
            TempData["Status"] = 400;
            TempData["Message"] = "Invalid request";
            return RedirectToAction("Error", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> DeleteProfile()
        {            
            var user = await _userService.CurrentUser();
            await _context.Adresses.Where(a => a.UserId == user.Id).ExecuteDeleteAsync();
            await _context.BasketProducts.Where(p => p.Basket.AspNetUserId == user.Id).ExecuteDeleteAsync();
            await _context.Baskets.Where(b => b.AspNetUserId == user.Id).ExecuteDeleteAsync();
            await _context.OrderProducts.Where(o => o.Order.UserId == user.Id).ExecuteDeleteAsync();
            await _context.StatusHistories.Where(s => s.Order.UserId == user.Id).ExecuteDeleteAsync();
            await _context.Orders.Where(o => o.UserId == user.Id).ExecuteDeleteAsync();
            await _context.Users.Where(u => u.Id == user.Id).ExecuteDeleteAsync();
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        [HttpGet]
        public async Task<IActionResult> Adress()
        {
            ViewBag.NavBar = 2;
            var user = await _userService.CurrentUser();
            var adress = await _context.Adresses.Where(a => a.UserId == user.Id).OrderByDescending(a => a.IsPrimary).ThenBy(a => a.Id).ToListAsync();
            var count = adress.Count();
            for (var i = 0; i < 3-count; i++)
            {
                adress.Add(new Adress());
            }
            return View(adress);
        }
        [HttpPost]
        public async Task<IActionResult> AddAdress(List<Adress> adresses)
        {
            var user = await _userService.CurrentUser();
            user.Adresses ??= new List<Adress>();
            if (ModelState.IsValid)
            {
                foreach (var model in adresses)
                {
                    var adress = model.Id == 0? new Adress(): await _context.Adresses.FirstOrDefaultAsync(a => a.Id == model.Id);
                    adress.UserId = user.Id;
                    adress.State = model.State;
                    adress.City = model.City;
                    adress.PostalCode = model.PostalCode;
                    adress.Street = model.Street;
                    adress.HouseNumber = model.HouseNumber;
                    adress.IsPrimary = model.IsPrimary;
                    if (model.Id == 0) { user.Adresses.Add(adress); }
                    else { _context.Adresses.Update(adress); }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Adress");
            }
            TempData["Status"] = 400;
            TempData["Message"] = "Invalid request";
            return RedirectToAction("Error", "Home");
        }
        public async Task<IActionResult> Orders()
        {
            ViewBag.NavBar = 3;
            var user = await _userService.CurrentUser();
            var order = await _context.Orders.Where(o => o.UserId == user.Id).Include(o => o.Products).ThenInclude(o => o.Product).Include(o => o.StatusHistories).OrderBy(o => o.CreatedDate).ToListAsync();
            return View(order);
        }
    }
}
