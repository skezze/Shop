using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Models;

namespace Shop.Data
{
        public class ApplicationDbContext:IdentityDbContext<User>
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<Product> Products { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderProduct> OrderProducts { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<User>()
                    .HasMany(x => x.Orders)
                    .WithOne(x => x.User)
                    .HasForeignKey(x => x.UserId)
                    .HasPrincipalKey(x => x.Id);
        
                modelBuilder.Entity<OrderProduct>()
                    .HasKey(x => new { x.OrderId, x.ProductId });
            }
        }
}
