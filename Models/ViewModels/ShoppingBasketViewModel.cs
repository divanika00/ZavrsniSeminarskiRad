using ZavrsniSeminarskiRad.Models.Base;

namespace ZavrsniSeminarskiRad.Models.ViewModels
{
    public class ShoppingBasketViewModel : ShoppingBasketBase
    {
        public int Id { get; set; }
        public List<ShoppingBasketItemViewModel> ShoppingBasketItems { get; set; }
        public AppUserViewModel AppUser { get; set; }
        public decimal GetTotalPrice()
        {
            decimal totalPrice = 0;

            if (ShoppingBasketItems == null)
            {
                return default;
            }
            if (ShoppingBasketItems.Any())
            {
                foreach (var item in ShoppingBasketItems)
                {
                    totalPrice += item.Price * item.Quantity;
                }

            }

            return totalPrice;
        }

        public decimal GetTotalPriceWithVAT()
        {
            decimal totalPrice = GetTotalPrice();
            if (totalPrice == default)
            {
                return default;
            }


            totalPrice = totalPrice * 1.25M;
            return totalPrice;
        }

        public ShoppingBasketStatus ShoppingBasketStatus { get; set; }
        public DateTime Created { get; set; }
    }
}
