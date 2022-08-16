using ZavrsniSeminarskiRad.Models.Base;
using ZavrsniSeminarskiRad.Models.Dbo.Base;

namespace ZavrsniSeminarskiRad.Models.Dbo
{
    public class ItemCategory : ItemCategoryBase, IEntityBase
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
    }
}
