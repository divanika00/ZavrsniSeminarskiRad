
using ZavrsniSeminarskiRad.Models.Binding;
using ZavrsniSeminarskiRad.Models.ViewModels;

namespace ZavrsniSeminarskiRad.Services.Interfaces
{
    public interface IServiceClient
    {
        //Task<TokenFeedback> GetToken(TokenLoginBinding model);
        Task<List<ItemCategoryViewModel>> ProductCategorys(string token);
        Task<List<ItemViewModel>> GetProducts(string token);
        Task<AppUserViewModel> GetUser(string token);
    }
}