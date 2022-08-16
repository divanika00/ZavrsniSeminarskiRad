namespace ZavrsniSeminarskiRad.Models
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string BasicUser = "BasicUser";
        public const string Employee = "Employee";
    }

    public static class CorsPolicy
    {
        public const string AllowAll = "AllowAllCors";
    }

    public enum ShoppingBasketStatus
    {
        Pending,
        Succeeded,
        Suspended

    }
}
