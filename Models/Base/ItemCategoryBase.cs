using System.ComponentModel.DataAnnotations;

namespace ZavrsniSeminarskiRad.Models.Base
{
    public abstract class ItemCategoryBase
    {
        [Required(ErrorMessage = "Obavezan unos!")]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
