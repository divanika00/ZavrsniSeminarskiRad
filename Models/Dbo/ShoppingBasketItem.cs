using ZavrsniSeminarskiRad.Models.Base;
using ZavrsniSeminarskiRad.Models.Dbo.Base;

namespace ZavrsniSeminarskiRad.Models.Dbo
{
    public class ShoppingBasketItem : ShoppingBasketItemBase, IEntityBase
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public Item Item { get; set; }
        public ShoppingBasket ShoppingBasket { get; set; }
    }
}
