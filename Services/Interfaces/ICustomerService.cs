using ZavrsniSeminarskiRad.Models.Binding;
using ZavrsniSeminarskiRad.Models.ViewModels;

namespace ZavrsniSeminarskiRad.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<AddressViewModel> AddAddress(AddressBinding model);
        Task<AddressViewModel> GetAddress(string userId);
        Task UpdateUserPhone(string userId, string phone);
    }
}