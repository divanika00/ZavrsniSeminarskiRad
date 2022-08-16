using ZavrsniSeminarskiRad.Models.Base;

namespace ZavrsniSeminarskiRad.Models.Binding
{
    public class ShoppingBasketApiBinding : ShoppingBasketBase
    {
        
        public int ItemId { get; set; }
       
        public decimal Quantity { get; set; }
       
        public decimal Price { get; set; }
       
        public int? ShoppingBasketId { get; set; }
    }
}
