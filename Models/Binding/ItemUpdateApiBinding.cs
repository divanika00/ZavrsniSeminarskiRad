using ZavrsniSeminarskiRad.Models.Base;

namespace ZavrsniSeminarskiRad.Models.Binding
{
    public class ItemUpdateApiBinding : ItemBase
    {
       
        public int Id { get; set; }
        
        public int ItemCategoryId { get; set; }
    }
}
