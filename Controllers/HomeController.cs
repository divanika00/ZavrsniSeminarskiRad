using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ZavrsniSeminarskiRad.Models;
using ZavrsniSeminarskiRad.Models.Binding;
using ZavrsniSeminarskiRad.Models.Dbo;
using ZavrsniSeminarskiRad.Services.Interfaces;

namespace ZavrsniSeminarskiRad.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IItemService itemService;
        private readonly UserManager<AppUser> userManager;
        public HomeController(ILogger<HomeController> logger, IItemService itemService, UserManager<AppUser> userManager)
        {
            this.itemService = itemService;
            this.userManager = userManager;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> SuspendShoppingBasketItem(int id)
        {
            await itemService.SuspendShoppingBasketItem(id);
            return RedirectToAction("ShoppingCart");
        }


        [HttpGet]
        public async Task<IActionResult> SuspendShoppingBasket(int id)
        {
            var shoppingCart = await itemService.SuspendShoppingBasket(id);
            return RedirectToAction("ShoppingCart");
        }


        public IActionResult Index()
        {
            return View(itemService.GetItemsAsync().Result);
        }

        [Authorize]
        public async Task<IActionResult> ItemView(int id)
        {
            var product = await itemService.GetItemAsync(id);

            return View(product);
        }


        [Authorize]
        public async Task<IActionResult> ShoppingBasket()
        {
            var shoppingCart = await itemService.GetShoppingBasketAsync(userManager.GetUserId(User));
            return View(shoppingCart);
        }

        

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ItemView(ShoppingBasketBinding model)
        {
            model.UserId = userManager.GetUserId(User);
            var product = await itemService.AddShoppingBasketAsync(model);
            return RedirectToAction("Index");
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