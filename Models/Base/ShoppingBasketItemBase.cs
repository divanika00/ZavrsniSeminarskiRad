using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZavrsniSeminarskiRad.Models.Base
{
    public abstract class ShoppingBasketItemBase
    {
        [Required]
        [Display(Name = "Količina")]
        [Column(TypeName = "decimal(9, 2)")] // Postoji još nekoliko načina za ASP.NET Core 5, ali ovo je najlakši. Prilagodite po potrebi
        public decimal Quantity { get; set; }

        [Required]
        [Display(Name = "Cijena")]
        [Column(TypeName = "decimal(9, 2)")]
        public decimal Price { get; set; }
    }
}
