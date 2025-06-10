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
            //this is to explicitly state the relationship between User
            //and Customer especially the AssignedSalesRepiId foreign key to User.Id
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.AssignedSalesRep)
            .WithMany(u => u.AssignedCustomers)
            .HasForeignKey(c => c.AssignedSalesRepId);
        modelBuilder.Entity<ContactHistory>()
            .HasOne(ch => ch.CreatedByUser)
            .WithMany(ch => ch.ContactHistoriesCreatedbyUser)
            .HasForeignKey(ch => ch.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "alice", Email = "alice@gmail.com", PasswordHash = "hash1", Role = Role.Admin, CreatedDate = new DateTime(2024, 1, 1) },
            new User { Id = 2, Username = "bob", Email = "bob@gmail.com", PasswordHash = "hash2", Role = Role.Manager, CreatedDate = new DateTime(2024, 2, 1) },
            new User { Id = 3, Username = "carol", Email = "carol@gmail.com", PasswordHash = "hash3", Role = Role.Salesman, CreatedDate = new DateTime(2024, 3, 1) },
            new User { Id = 4, Username = "dave", Email = "dave@gmail.com", PasswordHash = "hash4", Role = Role.DeveloperTester, CreatedDate = new DateTime(2024, 4, 1) },
            new User { Id = 5, Username = "eve", Email = "eve@gmail.com", PasswordHash = "hash5", Role = Role.Salesman, CreatedDate = new DateTime(2024, 5, 1) }
        );
        // Seed Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer { ID = 1, Name = "Acme Corp", Email = "contact.acme@gmail.com", Phone = "1234567890", Company = "Acme", CreatedDate = new DateTime(2024, 6, 1), LastContactDate = new DateTime(2024, 6, 5), Status = CustomerStatus.Active, Priority = CustomerPriority.High, AssignedSalesRepId = 3 },
            new Customer { ID = 2, Name = "Beta LLC", Email = "info.beta@gmail.com", Phone = "2345678901", Company = "Beta", CreatedDate = new DateTime(2024, 6, 2), LastContactDate = new DateTime(2024, 6, 6), Status = CustomerStatus.Prospect, Priority = CustomerPriority.Medium, AssignedSalesRepId = 5 },
            new Customer { ID = 3, Name = "Gamma Inc", Email = "hello.gamma@gmail.com", Phone = "3456789012", Company = "Gamma", CreatedDate = new DateTime(2024, 6, 3), LastContactDate = new DateTime(2024, 6, 7), Status = CustomerStatus.Inactive, Priority = CustomerPriority.Low, AssignedSalesRepId = 3 },
            new Customer { ID = 4, Name = "Delta Ltd", Email = "support.delta@gmail.com", Phone = "4567890123", Company = "Delta", CreatedDate = new DateTime(2024, 6, 4), LastContactDate = new DateTime(2024, 6, 8), Status = CustomerStatus.Active, Priority = CustomerPriority.Medium, AssignedSalesRepId = 5 },
            new Customer { ID = 5, Name = "Epsilon GmbH", Email = "contact.epsilon@gmail.com", Phone = "5678901234", Company = "Epsilon", CreatedDate = new DateTime(2024, 6, 5), LastContactDate = new DateTime(2024, 6, 9), Status = CustomerStatus.Prospect, Priority = CustomerPriority.High, AssignedSalesRepId = 2 }
        );
        // Seed ContactHistory
        modelBuilder.Entity<ContactHistory>().HasData(
            new ContactHistory { ContactHistoryID = 10, CustomerID = 1, ContactType = "Email", Notes = "Initial contact", ContactDate = new DateTime(2024, 6, 5), Outcome = "Interested", CreatedByUserId = 2 },
            new ContactHistory { ContactHistoryID = 20, CustomerID = 2, ContactType = "Phone", Notes = "Follow-up call", ContactDate = new DateTime(2024, 6, 6), Outcome = "No answer", CreatedByUserId = 3 },
            new ContactHistory { ContactHistoryID = 30, CustomerID = 3, ContactType = "Meeting", Notes = "Demo presented", ContactDate = new DateTime(2024, 6, 7), Outcome = "Considering", CreatedByUserId = 5 },
            new ContactHistory { ContactHistoryID = 40, CustomerID = 4, ContactType = "Email", Notes = "Sent proposal", ContactDate = new DateTime(2024, 6, 8), Outcome = "Waiting", CreatedByUserId = 3 },
            new ContactHistory { ContactHistoryID = 50, CustomerID = 5, ContactType = "Phone", Notes = "Negotiation", ContactDate = new DateTime(2024, 6, 9), Outcome = "Deal closed", CreatedByUserId = 2 }
        );
    }
    public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<ContactHistory> ContactHistories { get; set; }
    public DbSet<User> Users { get; set; }
}