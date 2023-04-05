using Mag.Auth;
using Mag.Interfaces;
using Mag.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Mag.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _context;
        private readonly ApplicationContext _dbContext;
        private readonly UserManager<AspNetUser> _userManager;

        public UserService(IHttpContextAccessor context, UserManager<AspNetUser> userManager, ApplicationContext dbContext)
        {
            _context = context;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<AspNetUser?> CurrentUser()
        {
            var user = _context.HttpContext?.User;
            if (user == null)
            {
                return null;
            }
            return await _userManager.GetUserAsync(user);
        }

        public string? GetEmail()
        {
            return _context.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }

        public string? GetUserName()
        {
            return _context.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        }
        public bool IsAuthorized()
        {
            return _context.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        }
        public async Task<int> BasketItems()
        {
            var user = await CurrentUser();
            var basket = await _dbContext.Baskets.Include(b => b.Products).ThenInclude(b => b.Products).FirstOrDefaultAsync(b => b.AspNetUserId == user.Id);
            return basket?.Products.GroupBy(p => p.ProductsId).Count() ?? 0;
        }
    }
}
