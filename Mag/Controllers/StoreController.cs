using Mag.Auth;
using Mag.Interfaces;
using Mag.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Mag.Models.Order;

namespace Mag.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IUserService _userService;

        public StoreController(ApplicationContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Categories()
        {
            return View(Enum.GetValues(typeof(Categories)).Cast<Categories>().ToList());
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Products(int? id)
        {
            if(id == null)
            {
                return View(await _context.Products.ToListAsync());
            }
            return View(await _context.Products.Where(p => p.Category == (Categories)id).ToListAsync());
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Product(int id)
        {
            var prod = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (prod == null)
            {
                return RedirectToAction("Products");
            }
            return View(prod);
        }
        public async Task<IActionResult> AddToBasket(int id)
        {
            var prod = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (prod == null)
            {
                return NotFound();
            }
            var user = await _userService.CurrentUser()!;
            var basket = await _context.Baskets.Include(b => b.BasketProducts).ThenInclude(b => b.Product).FirstOrDefaultAsync(b => b.AspNetUserId == user.Id);
            var newBasket = basket == null;
            basket ??= new Basket() { BasketProducts = new List<BasketProduct>(), AspNetUserId = user.Id };
            if (newBasket)
            {
                await _context.Baskets.AddAsync(basket);
                await _context.SaveChangesAsync();
            }
            basket.BasketProducts.Add(new BasketProduct { BasketId = basket.Id, ProductId = prod.Id, Product = prod });
            await _context.SaveChangesAsync();
            return RedirectToAction("Product", id);
        }
        [HttpGet]
        public async Task<IActionResult> ViewBasket()
        {
            var user = await _userService.CurrentUser()!;
            return View(await _context.Baskets.Include(b => b.BasketProducts).ThenInclude(b => b.Product).FirstOrDefaultAsync(b => b.AspNetUserId == user.Id));
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var user = await _userService.CurrentUser()!;
            await _context.BasketProducts.Where(p => p.ProductId == id && p.Basket.User.Id == user.Id).ExecuteDeleteAsync();
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
            var basket = await _context.Baskets.Include(b => b.BasketProducts).ThenInclude(b => b.Product).FirstOrDefaultAsync(b => b.AspNetUserId == user.Id);
            basket.BasketProducts.Add(new BasketProduct { BasketId = basket.Id, ProductId = prod.Id, Product = prod });
            await _context.SaveChangesAsync();
            return RedirectToAction("ViewBasket");
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            var user = await _userService.CurrentUser()!;
            await _context.BasketProducts.Where(p => p.ProductId == id && p.Basket.User.Id == user.Id).Take(1).ExecuteDeleteAsync();
            return RedirectToAction("ViewBasket");
        }
        [HttpGet]
        public async Task<IActionResult> ChooseAdress()
        {
            var user = await _userService.CurrentUser();
            return View(await _context.Adresses.Where(a => a.UserId == user.Id && a.City != null && a.City != "").ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> ChooseAdress(Adress adress)
        {
            TempData["Adress"] = adress.Id;
            return RedirectToAction("Checkout");
        }
        public async Task<IActionResult> Checkout()
        {
            var user = await _userService.CurrentUser()!;
            var adressId = TempData["Adress"];
            var basket = await _context.Baskets.Include(b => b.BasketProducts).ThenInclude(b => b.Product).FirstOrDefaultAsync(b => b.AspNetUserId == user.Id);
            var adress = await _context.Adresses.Where(a => a.UserId == user.Id && a.Id == (int)adressId).FirstOrDefaultAsync();
            ViewBag.adress = adress;
            ViewBag.basket = basket;
            return View();
        }
        public async Task<IActionResult> Order(int adressId)
        {
            var user = await _userService.CurrentUser();
            var basket = await _context.Baskets.Include(b => b.BasketProducts).ThenInclude(b => b.Product).FirstOrDefaultAsync(b => b.AspNetUserId == user.Id);
            var adress = await _context.Adresses.Where(a => a.UserId == user.Id && a.Id == (int)adressId).FirstOrDefaultAsync();
            if (basket == null || adress == null)
            {
                TempData["status"] = 400;
                TempData["Message"] = "Invalid request";
                return RedirectToAction("Error", "Home");
            }

            Order order = new()
            {
                UserId = user.Id,
                State = adress.State,
                City = adress.City,
                PostalCode = adress.PostalCode,
                Street = adress.Street,
                HouseNumber = adress.HouseNumber,
                Products = basket.BasketProducts.GroupBy(b => b.ProductId).Select(b => new OrderProduct
                {
                    ProductId = b.Key,
                    Price = b.FirstOrDefault().Product.Price,
                    Count = b.Count()
                }).ToList(),
                Status = StatusEnum.Pending,
                CreatedDate = DateTime.Now,
                StatusHistories = new List<StatusHistory>
                {
                    new StatusHistory { Status = StatusEnum.Pending, StatusChanged = DateTime.Now }
                }
            };
            await _context.Orders.AddAsync(order);
            await _context.Baskets.Where( b => b.Id == basket.Id ).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
