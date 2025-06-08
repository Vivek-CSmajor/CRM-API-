using Microsoft.EntityFrameworkCore;
using MockCRM.Models;

namespace MockCRM.Data;

public class CrmDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<ContactHistory>()
            .HasOne(ch => ch.Customer)
            .WithMany(c => c.ContactHistories)
            .HasForeignKey(ch => ch.CustomerID)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasDefaultValue(Role.Salesman);
    }
    public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<ContactHistory> ContactHistories { get; set; }
    public DbSet<User> Users { get; set; }
}