using System.Security.Claims;
using ZavrsniSeminarskiRad.Models.Binding;
using ZavrsniSeminarskiRad.Models.Dbo;
using ZavrsniSeminarskiRad.Models.ViewModels;

namespace ZavrsniSeminarskiRad.Services.Interfaces
{
    public interface IAppUserService
    {
        Task<AppUserViewModel?> CreateApiUserAsync(ApiBasicDataUser model);
        Task<AppUserViewModel?> CreateApiUserAsync(UserBinding model, string role);
        Task<AppUser?> CreateUserAsync(ApiBasicDataUser model, string role);
        Task<AppUserViewModel?> CreateUserAsync(AppUserAdminBinding model);
        Task<AppUser?> CreateUserAsync(UserBinding model, string role);
        Task DeleteUserAsync(AppUserAdminUpdateBinding model);
        Task DeleteUserAsync(string id);
        Task<AppUserViewModel> GetUser(ClaimsPrincipal user);
        Task<AppUserViewModel> GetUser(string id);
        Task<string> GetUserRole(string id);
        Task<List<AppUserRolesViewModel>> GetUserRoles();
        Task<List<AppUserViewModel>> GetUsers();
        Task<AppUserViewModel> UpdateUser(AppUserAdminUpdateBinding model);
    }
}