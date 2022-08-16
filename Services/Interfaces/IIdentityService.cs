using ZavrsniSeminarskiRad.Models.Dbo;

namespace ZavrsniSeminarskiRad.Services.Interfaces
{
    public interface IIdentityService
    {
        Task CreateRoleAsync(string role);
        Task CreateUserAsync(AppUser user, string password, string role);
    }
}