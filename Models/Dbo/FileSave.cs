using ZavrsniSeminarskiRad.Models.Base;
using ZavrsniSeminarskiRad.Models.Dbo.Base;

namespace ZavrsniSeminarskiRad.Models.Dbo
{
    public class FileSave : FileSaveBase, IEntityBase
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
    }
}
