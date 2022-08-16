using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ZavrsniSeminarskiRad.Data;
using ZavrsniSeminarskiRad.Models.Binding;
using ZavrsniSeminarskiRad.Models.Dbo;
using ZavrsniSeminarskiRad.Models.ViewModels;
using ZavrsniSeminarskiRad.Services.Interfaces;

namespace ZavrsniSeminarskiRad.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public CustomerService(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        /// <summary>
        /// Dohvati adresu korisnika
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<AddressViewModel> GetAddress(string userId)
        {
            var adress = await db.Address.FirstOrDefaultAsync(x => x.AppUser.Id == userId);
            return mapper.Map<AddressViewModel>(adress);

        }


        public async Task UpdateUserPhone(string userId, string phone)
        {
            var user = await db.AppUser.FirstOrDefaultAsync(x => x.Id == userId);
            user.PhoneNumber = phone;
            await db.SaveChangesAsync();

        }

        public async Task<AddressViewModel> AddAddress(AddressBinding model)
        {
            var user = await db.AppUser
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (user == null)
            {
                return null;
            }

            var dbo = mapper.Map<Address>(model);
            user.Address.Add(dbo);
            await db.SaveChangesAsync();
            return mapper.Map<AddressViewModel>(dbo);
        }
    }
}
