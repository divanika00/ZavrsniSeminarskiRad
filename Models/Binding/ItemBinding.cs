using System.ComponentModel.DataAnnotations;
using ZavrsniSeminarskiRad.Models.Base;

namespace ZavrsniSeminarskiRad.Models.Binding
{
    public class ItemBinding : ItemBase
    {
       
        public int ItemCategoryId { get; set; }
        [Display(Name = "Slika proizvoda")]
        public IFormFile ItemImg { get; set; }
    }
}
