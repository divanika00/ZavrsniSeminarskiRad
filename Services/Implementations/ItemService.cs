using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ZavrsniSeminarskiRad.Data;
using ZavrsniSeminarskiRad.Models;
using ZavrsniSeminarskiRad.Models.Binding;
using ZavrsniSeminarskiRad.Models.Dbo;
using ZavrsniSeminarskiRad.Models.ViewModels;
using ZavrsniSeminarskiRad.Services.Interfaces;

namespace ZavrsniSeminarskiRad.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly IFileSaveService fileSaveService;
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        private readonly ISharedService sharedService;
        public ItemService(ApplicationDbContext db,
            IMapper mapper, ISharedService sharedService,
            IFileSaveService fileSaveService)
        {

            this.db = db;
            this.mapper = mapper;
            this.sharedService = sharedService;
            this.fileSaveService = fileSaveService;
        }
        /// <summary>
        /// Obriši proizvod iz baze
        /// </summary>
        /// <param name="model">ItemUpdateBinding</param>
        /// <returns></returns>
        public async Task DeleteItemAsync(ItemUpdateBinding model)
        {
            var dbo = await db.Item
                .Include(x => x.ItemCategory)
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            await DeleteInDatabase(dbo);
        }

        public async Task DeleteInDatabase(Item item)
        {
            if (item != null)
            {
                db.Item.Remove(item);
            }
            await db.SaveChangesAsync();

            return;
        }

        public async Task SuspendShoppingBasketItem(int shoppingBasketItemId)
        {

            var shoppingBasketItem = await db.ShoppingBasketItem
                .Include(x => x.ShoppingBasket)
                .ThenInclude(x => x.ShoppingBasketItem)
                .FirstOrDefaultAsync(x => x.Id == shoppingBasketItemId);

            if (shoppingBasketItem == null)
            {
                return;
            }


            if (shoppingBasketItem.ShoppingBasket.ShoppingBasketItem.Count == 1)
            {
                await SuspendShoppingBasket(shoppingBasketItem.ShoppingBasket.Id);
                return;
            }


            try
            {
                shoppingBasketItem.ShoppingBasket.ShoppingBasketItem.Remove(shoppingBasketItem);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }



            return;

        }


        /// <summary>
        /// Storniraj košaricu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ShoppingBasketViewModel> SuspendShoppingBasket(int id)
        {
            var shoppingBasket = await db.ShoppingBasket
                .Include(x => x.ShoppingBasketItem)
                .ThenInclude(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (shoppingBasket == null)
            {
                return null;
            }

            SuspendShoppingBasket(shoppingBasket);
            await db.SaveChangesAsync();

            return mapper.Map<ShoppingBasketViewModel>(shoppingBasket);

        }

        /// <summary>
        /// Dohvati košarice prema statusu
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<List<ShoppingBasketViewModel>> GetShoppingBasketsAsync(ShoppingBasketStatus status)
        {
            var shoppingBasket = await db.ShoppingBasket
                .Include(x => x.ShoppingBasketItem)
                .ThenInclude(x => x.Item)
                .ThenInclude(x => x.ItemCategory)

                .Where(x => x.ShoppingBasketStatus == status).ToListAsync();

            if (!shoppingBasket.Any())
            {
                return new List<ShoppingBasketViewModel>();
            }

            return shoppingBasket.Select(x => mapper.Map<ShoppingBasketViewModel>(x)).ToList();

        }
        /// <summary>
        /// Dohvati košaricu na temelju id korisnika
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ShoppingBasketViewModel> GetShoppingBasketAsync(string userId)
        {
            var shoppingCart = await db.ShoppingBasket
                .Include(x => x.ShoppingBasketItem)
                .ThenInclude(x => x.Item)
                .ThenInclude(x => x.ItemCategory)

                .FirstOrDefaultAsync(x => x.AppUser.Id == userId && x.ShoppingBasketStatus == ShoppingBasketStatus.Pending);

            if (shoppingCart == null)
            {
                return null;
            }

            return mapper.Map<ShoppingBasketViewModel>(shoppingCart);

        }

        public async Task<ShoppingBasketViewModel> AddShoppingBasketAsync(ShoppingBasketApiBinding model, string userId)
        {
            //if (model.ShoppingCartId.HasValue)
            //{
            //    return await AddItemToShoppingCartAsync(model);
            //}

            var item = await db.Item.FindAsync(model.ItemId);
            item.Quantity -= model.Quantity;

            var user = await db.AppUser.FirstOrDefaultAsync(x => x.Id == userId);
            if (item == null || user == null)
            {
                return null;
            }

            var shoppingBasketItem = new ShoppingBasketItem
            {
                Price = model.Price,
                Item = item,
                Quantity = model.Quantity
            };


            var dbo = new ShoppingBasket
            {
                ShoppingBasketItem = new List<ShoppingBasketItem> { shoppingBasketItem },
                AppUser = user,
                ShoppingBasketStatus = ShoppingBasketStatus.Pending

            };
            db.ShoppingBasket.Add(dbo);
            await db.SaveChangesAsync();
            return mapper.Map<ShoppingBasketViewModel>(dbo);

        }

        /// <summary>
        /// Dodavanje nove košarice
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ShoppingBasketViewModel> AddShoppingBasketAsync(ShoppingBasketBinding model)
        {
            if (model.ShoppingBasketId.HasValue)
            {
                return await AddItemToShoppingBasketAsync(model);
            }

            var item = await db.Item.FindAsync(model.ItemId);
            item.Quantity -= model.Quantity;

            var user = await db.AppUser.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (item == null || user == null)
            {
                return null;
            }

            var shoppingBasketItem = new ShoppingBasketItem
            {
                Price = model.Price,
                Item = item,
                Quantity = model.Quantity
            };


            var dbo = new ShoppingBasket
            {
                ShoppingBasketItem = new List<ShoppingBasketItem> { shoppingBasketItem },
                AppUser = user,
                ShoppingBasketStatus = ShoppingBasketStatus.Pending

            };
            db.ShoppingBasket.Add(dbo);
            await db.SaveChangesAsync();
            return mapper.Map<ShoppingBasketViewModel>(dbo);

        }
        private async Task<ShoppingBasketViewModel> AddItemToShoppingBasketAsync(ShoppingBasketBinding model)
        {


            var dbo = await db.ShoppingBasket
                .Include(x => x.ShoppingBasketItem)
                .ThenInclude(x => x.Item)
                .FirstOrDefaultAsync(x => x.Id == model.ShoppingBasketId.GetValueOrDefault());
            var item = await db.Item.FindAsync(model.ItemId);
            item.Quantity -= model.Quantity;


            var presentShoppingBasketItem = dbo.ShoppingBasketItem.FirstOrDefault(x => x.Item.Id == model.ItemId);


            if (presentShoppingBasketItem != null)
            {
                presentShoppingBasketItem.Quantity += model.Quantity;
                await db.SaveChangesAsync();
                return mapper.Map<ShoppingBasketViewModel>(dbo);
            }


            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (item == null || user == null)
            {
                return null;
            }



            var shoppingCartItem = new ShoppingBasketItem
            {
                Price = model.Price,
                Item = item,
                Quantity = model.Quantity
            };

            if (dbo == null)
            {
                return null;
            }

            dbo.ShoppingBasketItem.Add(shoppingCartItem);

            await db.SaveChangesAsync();
            return mapper.Map<ShoppingBasketViewModel>(dbo);

        }
        /// <summary>
        /// Dodavanje proizvoda
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ItemViewModel> AddItemAsync(ItemBinding model)
        {
            var dbo = mapper.Map<Item>(model);

            if (model.ItemImg != null)
            {
                var fileResponse = await fileSaveService.AddFileToStorage(model.ItemImg);
                if (fileResponse != null)
                {
                    dbo.ProductImgUrl = fileResponse.DownloadUrl;
                }

            }


            var itemCategory = await db.ItemsCategory.FindAsync(model.ItemCategoryId);
            if (itemCategory == null)
            {
                return null;
            }
            dbo.ItemCategory = itemCategory;
            db.Item.Add(dbo);
            await db.SaveChangesAsync();
            return mapper.Map<ItemViewModel>(dbo);
        }
        /// <summary>
        /// Dohvati proizvod putem id-1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ItemViewModel> GetItemAsync(int id)
        {
            var dbo = await db.Item
                .Include(x => x.ItemCategory)
                .FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<ItemViewModel>(dbo);

        }
            
        /// <summary>
        /// Dohvati sve proizvode
        /// </summary>
        /// <returns></returns>
        public async Task<List<ItemViewModel>> GetItemsAsync()
        {
            var dbo = await db.Item
                .Include(x => x.ItemCategory)
                .ToListAsync();

            return dbo.Select(x => mapper.Map<ItemViewModel>(x)).ToList();
        }
        /// <summary>
        /// Dodaj kategoriju proizvoda
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ItemCategoryViewModel> AddItemCategoryAsync(ItemCategoryBinding model)
        {
            var dbo = mapper.Map<ItemCategory>(model);
            db.ItemsCategory.Add(dbo);
            await db.SaveChangesAsync();
            return mapper.Map<ItemCategoryViewModel>(dbo);
        }
        /// <summary>
        /// Dohvati kategoriju proivzvoda
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ItemCategoryViewModel> GetItemCategoryAsync(int id)
        {
            var dbo = await db.ItemsCategory.FindAsync(id);
            return mapper.Map<ItemCategoryViewModel>(dbo);

        }
        /// <summary>
        /// Dohvati sve kategorije proizvoda
        /// </summary>
        /// <returns></returns>
        public async Task<List<ItemCategoryViewModel>> GetItemCategorysAsync()
        {
            var dbo = await db.ItemsCategory.ToListAsync();
            return dbo.Select(x => mapper.Map<ItemCategoryViewModel>(x)).ToList();

        }
        /// <summary>
        /// Ako je shoppingcart u statusu active npr 2h.
        /// Prebaciti status u suspended
        /// Napraviti povrat robe na odg kolicinu
        /// </summary>
        /// <returns></returns>
        public async Task UpdateShoppinBasketStatus()
        {
            var shoppingCarts = await db.ShoppingBasket
                .Include(x => x.ShoppingBasketItem)
                .ThenInclude(x => x.Item)
                .Where(x => x.ShoppingBasketStatus == ShoppingBasketStatus.Pending && x.Created < DateTime.Now.AddHours(-2))
                .ToListAsync();

            if (!shoppingCarts.Any())
            {
                return;
            }

            SuspendShoppingBasket(shoppingCarts);

            await db.SaveChangesAsync();

        }

        private List<ShoppingBasket> SuspendShoppingBasket(List<ShoppingBasket> shoppingBaskets)
        {
            foreach (var shoppingBag in shoppingBaskets)
            {
                SuspendShoppingBasket(shoppingBag);
            }

            return shoppingBaskets;
        }

        private static ShoppingBasket SuspendShoppingBasket(ShoppingBasket shoppingBasket)
        {
            shoppingBasket.ShoppingBasketStatus = ShoppingBasketStatus.Suspended;
            foreach (var cartItems in shoppingBasket.ShoppingBasketItem)
            {
                cartItems.Item.Quantity += cartItems.Quantity;
            }

            return shoppingBasket;
        }

        public async Task<ItemViewModel> UpdateItemAsync(ItemUpdateApiBinding model)
        {
            var category = await db.ItemsCategory.FirstOrDefaultAsync(x => x.Id == model.ItemCategoryId);
            var dbo = await db.Item.FindAsync(model.Id);
            mapper.Map(model, dbo);
            dbo.ItemCategory = category;
            await db.SaveChangesAsync();
            return mapper.Map<ItemViewModel>(dbo);
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ItemViewModel> UpdateItemAsync(ItemUpdateBinding model)
        {
            var category = await db.ItemsCategory.FirstOrDefaultAsync(x => x.Id == model.ItemCategoryId);
            var dbo = await db.Item.FindAsync(model.Id);
            mapper.Map(model, dbo);
            dbo.ItemCategory = category;
            await db.SaveChangesAsync();
            return mapper.Map<ItemViewModel>(dbo);
        }
        public async Task<ItemCategoryViewModel> UpdateItemCategoryAsync(ItemCategoryUpdateBinding model)
        {
            var dbo = await db.ItemsCategory.FindAsync(model.Id);
            mapper.Map(model, dbo);
            await db.SaveChangesAsync();
            return mapper.Map<ItemCategoryViewModel>(dbo);
        }
        /// <summary>
        /// Dodaj predmet u košaricu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ShoppingBasketItemViewModel> AddShoppingBasketItemAsync(ShoppingBasketItemBinding model)
        {
            var dbo = mapper.Map<ShoppingBasketItem>(model);
            db.ShoppingBasketItem.Add(dbo);
            await db.SaveChangesAsync();
            return mapper.Map<ShoppingBasketItemViewModel>(dbo);
        }
        /// <summary>
        /// dohvati proizvod iz košarice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ShoppingBasketItemViewModel> GetShoppingBasketItemAsync(int id)
        {
            var dbo = await db.ShoppingBasketItem.FindAsync(id);
            return mapper.Map<ShoppingBasketItemViewModel>(dbo);

        }
        /// <summary>
        /// dohvati sve proizvode iz košarice
        /// </summary>
        /// <returns></returns>
        public async Task<List<ShoppingBasketItemViewModel>> GetShoppingBasketItemsAsync()
        {
            var dbo = await db.ShoppingBasketItem.ToListAsync();
            return dbo.Select(x => mapper.Map<ShoppingBasketItemViewModel>(x)).ToList();

        }
    }
}
