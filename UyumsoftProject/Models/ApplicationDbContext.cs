using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UyumsoftProject.Models;

namespace UyumsoftProject.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customizing the ASP.NET Identity model and overriding the defaults if needed
            builder.Entity<IdentityUserRole<string>>()
                   .HasOne<ApplicationRole>()
                   .WithMany()
                   .HasForeignKey(ur => ur.RoleId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
        public DbSet<UyumsoftProject.Models.Category> Categories { get; set; } = default!;
        public DbSet<UyumsoftProject.Models.Product> Products { get; set; } = default!;
        public DbSet<UyumsoftProject.Models.Brand> Brands { get; set; } = default!;
        public DbSet<UyumsoftProject.Models.Cart> Carts { get; set; } = default!;
        public DbSet<UyumsoftProject.Models.CartItem> CartItems { get; set; } = default!;
        public DbSet<UyumsoftProject.Models.Address> Address { get; set; } = default!;
        public DbSet<UyumsoftProject.Models.City> City { get; set; } = default!;
        public DbSet<UyumsoftProject.Models.Order> Orders { get; set; } = default!;
        public DbSet<UyumsoftProject.Models.OrderItem> OrderItems { get; set; } = default!;
    }
}
