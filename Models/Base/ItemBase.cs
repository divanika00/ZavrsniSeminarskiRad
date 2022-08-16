using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZavrsniSeminarskiRad.Models.Base
{
    public abstract class ItemBase
    {
        [Required]
        [StringLength(350, MinimumLength = 2)]
        [Display(Name = "Naziv")]
        public string Title { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Količina")]
        [Column(TypeName = "decimal(9, 2)")] 
        public decimal Quantity { get; set; }

        [Required]
        [Display(Name = "Cijena")]
        [Column(TypeName = "decimal(9, 2)")]
        public decimal Price { get; set; }

        public string? ProductImgUrl { get; set; }
    }
}
