using Mag.Auth;
using Mag.Interfaces;
using Mag.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mag.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IUserService _userService;

        public StoreController(ApplicationContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public IActionResult Products()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        public async Task<IActionResult> Product(int id)
        {
            var prod = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (prod == null)
            {
                return RedirectToAction("Products");
            }
            return View(prod);
        }
        [Authorize]
        public async Task<IActionResult> AddToBasket(int id)
        {
            var prod = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (prod == null)
            {
                return NotFound();
            }
            var user = await _userService.CurrentUser()!;
            var basket = await _context.Baskets.Include(b => b.Products).ThenInclude(b => b.Products).FirstOrDefaultAsync(b => b.AspNetUserId == user.Id);
            var newBasket = basket == null;
            basket ??= new Basket() { Products = new List<BasketProduct>(), AspNetUserId = user.Id };
            if (newBasket)
            {
                await _context.Baskets.AddAsync(basket);
                await _context.SaveChangesAsync();
            }
            basket.Products.Add(new BasketProduct { BasketsId = basket.Id, ProductsId = prod.Id, Products = prod });
            await _context.SaveChangesAsync();
            return RedirectToAction("Product", id);
        }
        [Authorize]
        public async Task<IActionResult> ViewBasket()
        {
            var user = await _userService.CurrentUser()!;
            return View(await _context.Baskets.Include(b => b.Products).ThenInclude(b => b.Products).FirstOrDefaultAsync(b => b.AspNetUserId == user.Id));
        }
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var user = await _userService.CurrentUser()!;
            await _context.BasketProducts.Where(p => p.ProductsId == id && p.Baskets.User.Id == user.Id).ExecuteDeleteAsync();
            return RedirectToAction("ViewBasket");
        }
        public async Task<IActionResult> AddProduct(int id)
        {
            var prod = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (prod == null)
            {
                return NotFound();
            }
            var user = await _userService.CurrentUser()!;
            var basket = await _context.Baskets.Include(b => b.Products).ThenInclude(b => b.Products).FirstOrDefaultAsync(b => b.AspNetUserId == user.Id);
            basket.Products.Add(new BasketProduct { BasketsId = basket.Id, ProductsId = prod.Id, Products = prod });
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewBasket");
        }
        public async Task<IActionResult> RemoveProduct(int id)
        {
            var user = await _userService.CurrentUser()!;
            await _context.BasketProducts.Where(p => p.ProductsId == id && p.Baskets.User.Id == user.Id).Take(1).ExecuteDeleteAsync();
            return RedirectToAction("ViewBasket");
        }
    }
}
