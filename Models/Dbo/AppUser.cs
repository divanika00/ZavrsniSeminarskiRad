using Microsoft.AspNetCore.Identity;

namespace ZavrsniSeminarskiRad.Models.Dbo
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Sourname { get; set; }
        public DateTime? DOB { get; set; }
        public ICollection<Address> Address { get; set; }
        public ICollection<ShoppingBasket> ShoppingBasket { get; set; }
    }
}
