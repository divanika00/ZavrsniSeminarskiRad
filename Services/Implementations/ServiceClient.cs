using IdentityModel.Client;
using Newtonsoft.Json;
using ZavrsniSeminarskiRad.Models.Binding;
using ZavrsniSeminarskiRad.Models.ViewModels;
using ZavrsniSeminarskiRad.Services.Interfaces;

namespace ZavrsniSeminarskiRad.Services.Implementations
{
    public class ServiceClient : ServiceClientBase, IServiceClient
    {
        public ServiceClient(HttpClient httpClient) : base(httpClient)
        {
        }

        //public async Task<TokenFeedback> GetToken(TokenLoginBinding model)
        //{
        //    string targetUrl = $"/api/UserApi/token";
        //    var response = await DoRequest(targetUrl, HttpMethod.Post, model);
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception("Dohvat tokena nije uspio");
        //    }
        //    var content = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<TokenFeedback>(content);
        //}

        public async Task<List<ItemCategoryViewModel>> ProductCategorys(string token)
        {
            string targetUrl = $"/api/AdminApi/product-categorys";
            var response = await DoRequest(targetUrl, HttpMethod.Get, token);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ItemCategoryViewModel>>(content);
        }

        public async Task<List<ItemViewModel>> GetProducts(string token)
        {
            string targetUrl = $"/api/homeapi/products";
            var response = await DoRequest(targetUrl, HttpMethod.Get, token);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ItemViewModel>>(content);
        }

        public async Task<AppUserViewModel> GetUser(string token)
        {
            string targetUrl = $"/api/HomeAPI/user-info";
            var response = await DoRequest(targetUrl, HttpMethod.Get, token);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AppUserViewModel>(content);
        }

    }
}
