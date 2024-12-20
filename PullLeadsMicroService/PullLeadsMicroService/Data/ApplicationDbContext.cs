using Microsoft.EntityFrameworkCore;
using PullLeadsMicroService.Models;

namespace PullLeadsMicroService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        // DbSet for the Leads table
        public DbSet<Lead> Leads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Leads table schema
            modelBuilder.Entity<Lead>(entity =>
            {
                entity.HasKey(e => e.Id); // Primary key
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd(); // Auto-increment
                entity.Property(e => e.EmailAddress)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.HasIndex(e => e.EmailAddress)
                      .IsUnique(); // Unique constraint on EmailAddress
                entity.Property(e => e.ContactPerson)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(e => e.InterestArea)
                      .IsRequired()
                      .HasMaxLength(255);
            });
        }
    }
}
