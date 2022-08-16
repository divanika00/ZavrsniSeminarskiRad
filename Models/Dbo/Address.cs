using System.ComponentModel.DataAnnotations;
using ZavrsniSeminarskiRad.Models.Base;
using ZavrsniSeminarskiRad.Models.Dbo.Base;

namespace ZavrsniSeminarskiRad.Models.Dbo
{
    public class Address : AddressBase, IEntityBase
    {
        [Key]
        public int Id { get; set; }
        public AppUser AppUser { get; set; }
        public DateTime Created { get; set; }
    }
}
