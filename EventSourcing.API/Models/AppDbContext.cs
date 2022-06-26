using Microsoft.EntityFrameworkCore;

namespace EventSourcing.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");


            modelBuilder.Entity<Product>(e =>
            {
                e.HasIndex(p => p.Id).IsUnique();
                e.Property(p => p.Name).HasColumnType("nvarchar(500)").IsRequired();
                e.Property(p => p.Price).HasDefaultValue(0).HasColumnType("decimal(18,2)").IsRequired();
                e.Property(p => p.Stock).HasDefaultValue(0).IsRequired();
            });
        }

        public DbSet<Product> Products { get; set; }   
    }
}
