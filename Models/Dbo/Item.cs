using ZavrsniSeminarskiRad.Models.Base;
using ZavrsniSeminarskiRad.Models.Dbo.Base;

namespace ZavrsniSeminarskiRad.Models.Dbo
{
    public class Item : ItemBase, IEntityBase
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public ItemCategory ItemCategory { get; set; }


    }
}
