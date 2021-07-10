using DreamFishingNew.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DreamFishingNew.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Bag> Bags { get; set; }

        public DbSet<Bait> Baits { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Clothes> Clothes { get; set; }

        public DbSet<Glasses> Glasses { get; set; }

        public DbSet<Line> Lines { get; set; }

        public DbSet<Meter> Meters { get; set; }

        public DbSet<ProductCart> ProductCarts { get; set; }

        public DbSet<Reel> Reels { get; set; }

        public DbSet<Rod> Rods { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Bag>()
            .Property(b => b.Price)
            .HasPrecision(14, 2);

            builder.Entity<Bait>()
            .Property(b => b.Price)
            .HasPrecision(14, 2);

            builder.Entity<Clothes>()
            .Property(b => b.Price)
            .HasPrecision(14, 2);

            builder.Entity<Glasses>()
            .Property(b => b.Price)
            .HasPrecision(14, 2);

            builder.Entity<Line>()
            .Property(b => b.Price)
            .HasPrecision(14, 2);

            builder.Entity<Meter>()
            .Property(b => b.Price)
            .HasPrecision(14, 2);

            builder.Entity<Reel>()
            .Property(b => b.Price)
            .HasPrecision(14, 2);

            builder.Entity<Rod>()
            .Property(b => b.Price)
            .HasPrecision(14, 2);
        }
    }
}
