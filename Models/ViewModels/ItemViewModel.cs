using ZavrsniSeminarskiRad.Models.Base;

namespace ZavrsniSeminarskiRad.Models.ViewModels
{
    public class ItemViewModel : ItemBase
    {
        public int Id { get; set; }
        public ItemCategoryViewModel ItemCategory { get; set; }
    }
}
