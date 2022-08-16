using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ZavrsniSeminarskiRad.Models.Binding;
using ZavrsniSeminarskiRad.Models.Dbo;
using ZavrsniSeminarskiRad.Models.ViewModels;

namespace ZavrsniSeminarskiRad.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IdentityRole, AppUserRolesViewModel>();
            CreateMap<ItemBinding, Item>();
            CreateMap<Item, ItemViewModel>();
            CreateMap<ItemCategoryBinding, ItemCategory>();
            CreateMap<ItemCategory, ItemCategoryViewModel>();
            CreateMap<ItemCategoryUpdateBinding, ItemCategory>();
            CreateMap<ShoppingBasketItemBinding, ShoppingBasketItem>();
            CreateMap<ShoppingBasketItem, ShoppingBasketItemViewModel>();
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<ShoppingBasket, ShoppingBasketViewModel>();
            

            CreateMap<ItemViewModel, ItemUpdateBinding>();
            CreateMap<ItemUpdateBinding, Item>();
            CreateMap<ItemUpdateApiBinding, Item>();


            CreateMap<AddressBinding, Address>();
            CreateMap<Address, AddressViewModel>();
            CreateMap<UserBinding, AppUser>()
                .ForMember(dst => dst.UserName, opts => opts.MapFrom(src => src.Email))
                .ForMember(dst => dst.EmailConfirmed, opts => opts.MapFrom(src => true));


            CreateMap<FileSave, FileSaveViewModel>();
            CreateMap<FileSave, FileSaveExpendedViewModel>();


            CreateMap<FileSaveViewModel, FileSave>().
                ForMember(dst => dst.Id, opts => opts.Ignore());
        }
    }
}
