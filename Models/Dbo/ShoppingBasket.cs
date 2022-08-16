using ZavrsniSeminarskiRad.Models.Base;
using ZavrsniSeminarskiRad.Models.Dbo.Base;

namespace ZavrsniSeminarskiRad.Models.Dbo
{
    public class ShoppingBasket : ShoppingBasketBase, IEntityBase
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public ICollection<ShoppingBasketItem> ShoppingBasketItem { get; set; }
        public AppUser AppUser { get; set; }
        public ShoppingBasketStatus ShoppingBasketStatus { get; set; }
    }
}
