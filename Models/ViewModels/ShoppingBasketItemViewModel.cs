using ZavrsniSeminarskiRad.Models.Base;

namespace ZavrsniSeminarskiRad.Models.ViewModels
{
    public class ShoppingBasketItemViewModel : ShoppingBasketItemBase
    {
        public int Id { get; set; }
        public ItemViewModel Item { get; set; }
    }
}
