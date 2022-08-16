using ZavrsniSeminarskiRad.Models.Base;

namespace ZavrsniSeminarskiRad.Models.ViewModels
{
    public class AppUserViewModel : AppUserBase
    {
        public string Id { get; set; }

        public string Role { get; set; }
        public List<AddressViewModel> Address { get; set; }
    }
}
