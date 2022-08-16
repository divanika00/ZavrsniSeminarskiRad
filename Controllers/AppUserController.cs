using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZavrsniSeminarskiRad.Models;
using ZavrsniSeminarskiRad.Models.Binding;
using ZavrsniSeminarskiRad.Models.Dbo;
using ZavrsniSeminarskiRad.Services.Interfaces;

namespace ZavrsniSeminarskiRad.Controllers
{
    public class AppUserController : Controller
    {
        private readonly IAppUserService appUserSevice;
        private readonly SignInManager<AppUser> signInManager;
        public AppUserController(IAppUserService appUserSevice, SignInManager<AppUser> signInManager)
        {
            this.appUserSevice = appUserSevice;
            this.signInManager = signInManager;
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(UserBinding model)
        {
            var result = await appUserSevice.CreateUserAsync(model, Roles.BasicUser);
            if (result != null)
            {
                
                await signInManager.SignInAsync(result, true);
                return RedirectToAction("Index", "Home");
            }


            return View();
        }
    }
}
