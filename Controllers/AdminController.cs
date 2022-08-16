using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZavrsniSeminarskiRad.Models;
using ZavrsniSeminarskiRad.Models.Binding;
using ZavrsniSeminarskiRad.Models.Dbo;
using ZavrsniSeminarskiRad.Models.ViewModels;
using ZavrsniSeminarskiRad.Services.Interfaces;

namespace ZavrsniSeminarskiRad.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly IItemService itemService;
        private readonly IMapper mapper;
        private readonly IAppUserService appUserSevice;

        public AdminController(IItemService itemService, IMapper mapper, IAppUserService appUserSevice)
        {
            this.appUserSevice = appUserSevice;
            this.mapper = mapper;
            this.itemService = itemService;

        }

        /// <summary>
        /// USER USER USER USER
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = await appUserSevice.GetUsers();
            return View(users);
        }


        //create USER
        [HttpGet]
        public async Task<IActionResult> CreateNewUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewUser(AppUserAdminBinding model)
        {
            await appUserSevice.CreateUserAsync(model);
            return RedirectToAction("Users");
        }


        //Edit

        [HttpGet]
        public async Task<IActionResult> UpdateUser(string id)
        {
            var user = await appUserSevice.GetUser(id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(AppUserAdminUpdateBinding model)
        {
            await appUserSevice.UpdateUser(model);
            return RedirectToAction("Users");
        }

        //detals
        [HttpGet]
        public async Task<IActionResult> UserDetails(string id)
        {
            var userDtls = await appUserSevice.GetUser(id);

            if (userDtls == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(userDtls);
        }


        //delete

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await appUserSevice.GetUser(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(AppUserAdminUpdateBinding model)
        {

            await appUserSevice.DeleteUserAsync(model);
            return RedirectToAction("Users");
        }














        /// <summary>
        /// PRODUCT PRODUCT PRODUCT PRODUCT
        /// </summary>
        /// <returns></returns>


        //products

        [HttpGet]
        public async Task<IActionResult> ItemAdministration()
        {
            var products = await itemService.GetItemsAsync();
            return View(products);
        }

        // add item
        [HttpGet]
        public async Task<IActionResult> AddItem()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(ItemBinding model)
        {
            await itemService.AddItemAsync(model);
            return RedirectToAction("ItemAdministration");
        }




        //add item category

        [HttpGet]
        public async Task<IActionResult> AddItemCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProductCategory(ItemCategoryBinding model)
        {
            await itemService.AddItemCategoryAsync(model);
            return RedirectToAction("ItemAdministration");
        }



        //update item

        [HttpGet]
        public async Task<IActionResult> UpdateItem(int id)
        {
            var product = await itemService.GetItemAsync(id);
            var model = mapper.Map<ItemUpdateBinding>(product);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateItem(ItemUpdateBinding model)
        {
            var product = await itemService.UpdateItemAsync(model);
            return RedirectToAction("ItemAdministration");
        }


        //details

        public async Task<IActionResult> ItemDetails(int id)
        {

            var itemDetails = await itemService.GetItemAsync(id);

            if (itemDetails == null) return View("NotFound");
            return View(itemDetails);
        }


        //delete


        [HttpGet]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var user = await itemService.GetItemAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(ItemUpdateBinding model)
        {

            await itemService.DeleteItemAsync(model);
            return RedirectToAction("ItemAdministration");
        }







    }
}
