using Microsoft.AspNetCore.Identity;
using ZavrsniSeminarskiRad.Models;
using ZavrsniSeminarskiRad.Models.Dbo;
using ZavrsniSeminarskiRad.Services.Interfaces;

namespace ZavrsniSeminarskiRad.Services.Implementations
{
    public class IdentityService : IIdentityService
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;

        public IdentityService(IServiceScopeFactory scopeFactory)
        {

            using (var scope = scopeFactory.CreateScope())
            {
                this.userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                this.roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


                CreateRoleAsync(Roles.Admin).Wait();
                CreateRoleAsync(Roles.BasicUser).Wait();
                CreateRoleAsync(Roles.Employee).Wait();

                //CreateUser()
                CreateUserAsync(new AppUser
                {
                    Name = "domagoj",
                    Sourname = "ivanika",
                    Email = "admintest@test.com",
                    UserName = "admintest@test.com",
                    DOB = DateTime.Now.AddYears(-35),
                    PhoneNumber = "+3859912345678",
                    Address = new List<Address>
                    {
                        new Address
                        {
                            City = "Vg",
                            Street = "Zagrebacka",
                        }
                    }

                }, "Pa$$word321", Roles.Admin).Wait();

                CreateUserAsync(new AppUser
                {
                    Name = "domagoj",
                    Sourname = "ivanika",
                    Email = "employeetest@test.com",
                    UserName = "employeetest@test.com",
                    DOB = DateTime.Now.AddYears(-35),
                    PhoneNumber = "+3859912345678",
                    Address = new List<Address>
                    {
                        new Address
                        {
                            City = "Vg",
                            Street = "Zagrebacka",
                        }
                    }

                }, "Pa$$word321", Roles.Employee).Wait();

                CreateUserAsync(new AppUser
                {
                    Name = "domagoj",
                    Sourname = "ivanika",
                    Email = "usertest@test.com",
                    UserName = "usertest@test.com",
                    DOB = DateTime.Now.AddYears(-35),
                    PhoneNumber = "+3859912345278",
                    Address = new List<Address>
                    {
                        new Address
                        {
                            City = "Vg",
                            Street = "Zagrebacka",
                        }
                    }

                }, "Pa$$w0rd", Roles.BasicUser).Wait();
            }


        }


        public async Task CreateRoleAsync(string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = role

                });
            }

        }


        public async Task CreateUserAsync(AppUser user, string password, string role)
        {

            //Prvo provjeri ima li korisnika sa istim emailom u bazi
            var find = await userManager.FindByEmailAsync(user.Email);
            if (find != null)
            {
                return;
            }


            user.EmailConfirmed = true;
            //Izraditi novog korisnika
            var createdUser = await userManager.CreateAsync(user, password);

            //Provjeriti jeli korisnik uspješno dodan
            if (createdUser.Succeeded)
            {
                //Dodati korisnika u rolu
                var userAddedToRole = await userManager.AddToRoleAsync(user, role);
                if (!userAddedToRole.Succeeded)
                {
                    throw new Exception("Korisnik nije dodan u rolu!");
                }
            }



        }
    }
}
