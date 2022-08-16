

using ZavrsniSeminarskiRad.Models.Base;

namespace ZavrsniSeminarskiRad.Models.Binding
{
    public class UserBinding : AppUserBinding
    {
        public AddressBinding UserAdress { get; set; }
    }
    public class AppUserBinding 
    {
        //Validirati empty
        public string Name { get; set; }
        //Validirati empty
        public string Sourname { get; set; }
        //Validirati DOB min 18g
        public DateTime DOB { get; set; }
       
        public string Email { get; set; }
       
        public string Password { get; set; }
        public AddressBinding UserAddress { get; set; }
    }
    public class AppUserAdminBinding : AppUserBinding
    {
        public string RoleId { get; set; }
    }

    public class AppUserAdminUpdateBinding : AppUserAdminBinding
    {
        public string Id { get; set; }
    }
}
