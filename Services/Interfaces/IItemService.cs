using ZavrsniSeminarskiRad.Models;
using ZavrsniSeminarskiRad.Models.Binding;
using ZavrsniSeminarskiRad.Models.Dbo;
using ZavrsniSeminarskiRad.Models.ViewModels;

namespace ZavrsniSeminarskiRad.Services.Interfaces
{
    public interface IItemService
    {
        Task DeleteInDatabase(Item item);
        Task DeleteItemAsync(ItemUpdateBinding model);
        Task<ItemViewModel> AddItemAsync(ItemBinding model);
        Task<ItemCategoryViewModel> AddItemCategoryAsync(ItemCategoryBinding model);
        Task<ShoppingBasketViewModel> AddShoppingBasketAsync(ShoppingBasketApiBinding model, string userId);
        Task<ShoppingBasketViewModel> AddShoppingBasketAsync(ShoppingBasketBinding model);
        Task<ShoppingBasketItemViewModel> AddShoppingBasketItemAsync(ShoppingBasketItemBinding model);
        Task<ItemViewModel> GetItemAsync(int id);
        Task<ItemCategoryViewModel> GetItemCategoryAsync(int id);
        Task<List<ItemCategoryViewModel>> GetItemCategorysAsync();
        Task<List<ItemViewModel>> GetItemsAsync();
        Task<ShoppingBasketViewModel> GetShoppingBasketAsync(string userId);
        Task<ShoppingBasketItemViewModel> GetShoppingBasketItemAsync(int id);
        Task<List<ShoppingBasketItemViewModel>> GetShoppingBasketItemsAsync();
        Task<List<ShoppingBasketViewModel>> GetShoppingBasketsAsync(ShoppingBasketStatus status);
        Task<ShoppingBasketViewModel> SuspendShoppingBasket(int id);
        Task SuspendShoppingBasketItem(int shoppingBasketItemId);
        Task<ItemViewModel> UpdateItemAsync(ItemUpdateApiBinding model);
        Task<ItemViewModel> UpdateItemAsync(ItemUpdateBinding model);
        Task<ItemCategoryViewModel> UpdateItemCategoryAsync(ItemCategoryUpdateBinding model);
        Task UpdateShoppinBasketStatus();
    }
}