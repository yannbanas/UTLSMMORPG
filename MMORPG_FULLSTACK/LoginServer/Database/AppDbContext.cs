using LoginServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginServer.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Token)
                .IsUnique();

            // Configuration pour la propriété Authority
            modelBuilder.Entity<User>()
                .Property(u => u.Authority)
                .HasDefaultValue("P");

            modelBuilder.Entity<User>()
                .Property(u => u.Created)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}