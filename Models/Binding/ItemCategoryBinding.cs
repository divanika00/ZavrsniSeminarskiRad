using ZavrsniSeminarskiRad.Models.Base;

namespace ZavrsniSeminarskiRad.Models.Binding
{
    public class ItemCategoryBinding : ItemCategoryBase
    {
    }
    public class ItemCategoryUpdateBinding : ItemCategoryBinding
    {
       
        public int Id { get; set; }
    }
}
