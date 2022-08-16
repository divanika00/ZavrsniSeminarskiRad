using ZavrsniSeminarskiRad.Models.Base;
using ZavrsniSeminarskiRad.Models.ViewModels;

namespace ZavrsniSeminarskiRad.Models.Binding
{
    public class ItemUpdateBinding : ItemBase
    {
       
        public int Id { get; set; }
        public ItemCategoryViewModel ItemCategory { get; set; }
       
        public int ItemCategoryId { get; set; }
    }
}
