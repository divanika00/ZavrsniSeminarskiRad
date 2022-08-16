using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZavrsniSeminarskiRad.Data;
using ZavrsniSeminarskiRad.Models;
using ZavrsniSeminarskiRad.Models.Binding;
using ZavrsniSeminarskiRad.Models.Dbo;
using ZavrsniSeminarskiRad.Models.ViewModels;
using ZavrsniSeminarskiRad.Services.Interfaces;

namespace ZavrsniSeminarskiRad.Services.Implementations
{
    public class AppUserService : IAppUserService
    {
        private readonly UserManager<AppUser> appUserManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        private SignInManager<AppUser> signInManager;
        private readonly ApplicationDbContext db;



        public AppUserService(UserManager<AppUser> appUserManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper, SignInManager<AppUser> signInManager, ApplicationDbContext db)
        {
            this.appUserManager = appUserManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.signInManager = signInManager;
            this.db = db;
        }

        public async Task<List<AppUserRolesViewModel>> GetUserRoles()
        {

            var roles = await db.Roles.ToListAsync();
            if (!roles.Any())
            {
                return new List<AppUserRolesViewModel>();
            }

            return roles.Select(x => mapper.Map<AppUserRolesViewModel>(x)).ToList();
        }
        public async Task<List<AppUserViewModel>> GetUsers()
        {
            var dboUsers = await db.AppUser
                .Include(x => x.Address)
                .ToListAsync();
            var response = dboUsers.Select(x => mapper.Map<AppUserViewModel>(x)).ToList();
            response.ForEach(x => x.Role = GetUserRole(x.Id).Result);
            return response;

        }
        public async Task<string> GetUserRole(string id)
        {
            var dboUser = await db.AppUser.FindAsync(id);
            if (dboUser == null)
            {
                return String.Empty;
            }
            var roles = await appUserManager.GetRolesAsync(dboUser);
            return roles.First();

        }
        public async Task<AppUserViewModel> UpdateUser(AppUserAdminUpdateBinding model)
        {
            var dboUser = await db.AppUser
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            var role = await db.Roles.FindAsync(model.RoleId);


            if (dboUser == null || role == null)
            {
                return null;
            }


            await DeleteAllUserRoles(dboUser);
            await appUserManager.AddToRoleAsync(dboUser, role.Name);

            dboUser.Name = model.Name;
            dboUser.Sourname = model.Sourname;
            dboUser.DOB = model.DOB;
            await db.SaveChangesAsync();


            var response = mapper.Map<AppUserViewModel>(dboUser);
            return response;
        }
        private async Task DeleteAllUserRoles(AppUser user)
        {
            var userRoles = await appUserManager.GetRolesAsync(user);
            foreach (var item in userRoles)
            {
                await appUserManager.RemoveFromRoleAsync(user, item);
            }



        }
        public async Task<AppUserViewModel> GetUser(string id)
        {
            var dboUser = await db.AppUser
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dboUser == null)
            {
                return null;
            }

            var response = mapper.Map<AppUserViewModel>(dboUser);
            response.Role = await GetUserRole(id);
            return response;
        }
        //public async Task<AppUserViewModel> GetUser(dboUser)
        //{

        //    var userId = appUserManager.GetUserId();
        //    return await GetUser(userId);

        //}
        public async Task<AppUserViewModel> GetUser(ClaimsPrincipal user)
        {
            var userId = appUserManager.GetUserId(user);
            return await GetUser(userId);

        }
        public async Task<AppUserViewModel?> CreateApiUserAsync(UserBinding model, string role)
        {
            var result = await CreateUserAsync(model, role);
            if (result == null)
            {
                return null;
            }
            return mapper.Map<AppUserViewModel>(result);

        }
        public async Task<AppUserViewModel?> CreateApiUserAsync(ApiBasicDataUser model)
        {
            var result = await CreateUserAsync(model, Roles.BasicUser);
            if (result == null)
            {
                return null;
            }
            return mapper.Map<AppUserViewModel>(result);

        }
        //public async Task DeleteUserAsync()
        //{

        //    //await appUserManager.DeleteAsync(user);

        //    await DeleteUserAsync(user);
        //    return;
        //}
        public async Task DeleteUserAsync(string id)
        {
            var user = await db.AppUser
                .FindAsync(id);
            //await appUserManager.DeleteAsync(user);

            await appUserManager.DeleteAsync(user);
            return;
        }
        public async Task DeleteUserAsync(AppUserAdminUpdateBinding model)
        {

            var dboUser = await db.AppUser
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            await DeleteInDatabase(dboUser);    

            return;
        }
        public async Task DeleteInDatabase(AppUser user)
        {
            if (user != null)
            {
                db.AppUser.Remove(user);
            }
            await db.SaveChangesAsync();

            return;
        }
        public async Task<AppUserViewModel?> CreateUserAsync(AppUserAdminBinding model)
        {
            var find = await appUserManager.FindByEmailAsync(model.Email);
            if (find != null)
            {
                return null;
            }

            var user = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Name,
                Sourname = model.Sourname,
                DOB = model.DOB

            };

            var roles = await GetUserRoles();
            var userRole = roles.FirstOrDefault(x => x.Id == model.RoleId);

            var createdUser = await appUserManager.CreateAsync(user, model.Password);
            if (createdUser.Succeeded)
            {
                var userAddedToRole = await appUserManager.AddToRoleAsync(user, userRole.Name);
                if (!userAddedToRole.Succeeded)
                {
                    throw new Exception("Korisnik nije dodan u rolu!");
                }
            }
            return mapper.Map<AppUserViewModel>(user);
        }
        public async Task<AppUser?> CreateUserAsync(ApiBasicDataUser model, string role)
        {
            var find = await appUserManager.FindByEmailAsync(model.Email);
            if (find != null)
            {
                return null;
            }

            var user = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
            };


            var createdUser = await appUserManager.CreateAsync(user, model.Password);
            if (createdUser.Succeeded)
            {
                var userAddedToRole = await appUserManager.AddToRoleAsync(user, role);
                if (!userAddedToRole.Succeeded)
                {
                    throw new Exception("Korisnik nije dodan u rolu!");
                }
            }
            return user;
        }
        public async Task<AppUser?> CreateUserAsync(UserBinding model, string role)
        {
            var find = await appUserManager.FindByEmailAsync(model.Email);
            if (find != null)
            {
                return null;
            }
            var user = mapper.Map<AppUser>(model);
            var adress = mapper.Map<Address>(model.UserAdress);
            user.Address = new List<Address>() { adress };
            var createdUser = await appUserManager.CreateAsync(user, model.Password);
            if (createdUser.Succeeded)
            {
                var userAddedToRole = await appUserManager.AddToRoleAsync(user, role);
                if (!userAddedToRole.Succeeded)
                {
                    throw new Exception("Korisnik nije dodan u rolu!");
                }
            }
            return user;
        }
    }
}
