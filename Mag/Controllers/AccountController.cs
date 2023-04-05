using Mag.Auth;
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
    [AllowAnonymous]
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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["status"] = 400;
                TempData["Message"] = "Invalid requst";
                return RedirectToAction("Error", "Home");
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["status"] = 401;
                TempData["Message"] = "Email or password is invalid";
                return RedirectToAction("Error", "Home");
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
            {
                TempData["status"] = 401;
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
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByNameAsync(model.Email);
                if (userExists != null)
                {
                    TempData["status"] = 400;
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
                    TempData["status"] = 400;
                    TempData["Message"] = $"User creation failed! {errorMessage}";
                    return RedirectToAction("Error", "Home");
                }
                return RedirectToAction("Login");
            }
            TempData["status"] = 400;
            TempData["Message"] = "Invalid request";
            return RedirectToAction("Error", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        public async Task<IActionResult> Profile()
        {
            return View(await _userService.CurrentUser());
        }
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
            TempData["status"] = 400;
            TempData["Message"] = "Invalid request";
            return RedirectToAction("Error", "Home");
        }
    }
}
