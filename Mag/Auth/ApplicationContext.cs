using Mag.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mag.Auth
{
    public class ApplicationContext : IdentityDbContext<AspNetUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        { }
        public virtual DbSet<Product> Products => Set<Product>();
        public virtual DbSet<Basket> Baskets => Set<Basket>();
        public virtual DbSet<BasketProduct> BasketProducts => Set<BasketProduct>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.ToTable("AspNetUsers");
            });
        }


    }
}
