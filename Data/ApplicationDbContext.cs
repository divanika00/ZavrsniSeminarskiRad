using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZavrsniSeminarskiRad.Models.Dbo;
using ZavrsniSeminarskiRad.Models.Dbo.Base;

namespace ZavrsniSeminarskiRad.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override int SaveChanges()
        {

            var entries = ChangeTracker
                        .Entries()
                        .Where(e => e.Entity is IEntityBase && (
                          e.State == EntityState.Added
                          || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        ((IEntityBase)entityEntry.Entity).Created = DateTime.Now;
                        break;
                    default:
                        break;
                }

            }
            return base.SaveChanges();

        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {

            var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IEntityBase && (
              e.State == EntityState.Added
              || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        ((IEntityBase)entityEntry.Entity).Created = DateTime.Now;
                        break;
                    default:
                        break;
                }

            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }



        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<ItemCategory> ItemsCategory { get; set; }
        public DbSet<ShoppingBasketItem> ShoppingBasketItem { get; set; }
        public DbSet<ShoppingBasket> ShoppingBasket { get; set; }        
        public DbSet<FileSave> FileSave { get; set; }
    }
}