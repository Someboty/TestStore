using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mag.Models
{
    public class ApplicationContext : IdentityDbContext<AspNetUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        { }
        public virtual DbSet<Product> Products => Set<Product>();
        public virtual DbSet<Basket> Baskets => Set<Basket>();
        public virtual DbSet<BasketProduct> BasketProducts => Set<BasketProduct>();
        public virtual DbSet<Adress> Adresses => Set<Adress>();
        public virtual DbSet<Order> Orders => Set<Order>();
        public virtual DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();
        public virtual DbSet<StatusHistory> StatusHistories => Set<StatusHistory>();
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
