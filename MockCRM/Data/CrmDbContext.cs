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
            new Customer { ID = 1, Name = "Acme Corp", Email = "contact.acme@gmail.com", Phone = "+91 9123456789", Company = "Acme", CreatedDate = new DateTime(2024, 6, 1), LastContactDate = new DateTime(2024, 6, 5), Status = CustomerStatus.Active, Priority = CustomerPriority.High, AssignedSalesRepId = 3, Revenue = 100000 },
            new Customer { ID = 2, Name = "Beta LLC", Email = "info.beta@gmail.com", Phone = "+91 9234567890", Company = "Beta", CreatedDate = new DateTime(2024, 6, 2), LastContactDate = new DateTime(2024, 6, 6), Status = CustomerStatus.Prospect, Priority = CustomerPriority.Medium, AssignedSalesRepId = 5, Revenue = 50000 },
            new Customer { ID = 3, Name = "Gamma Inc", Email = "hello.gamma@gmail.com", Phone = "+91 9345678901", Company = "Gamma", CreatedDate = new DateTime(2024, 6, 3), LastContactDate = new DateTime(2024, 6, 7), Status = CustomerStatus.Inactive, Priority = CustomerPriority.Low, AssignedSalesRepId = 3, Revenue = null },
            new Customer { ID = 4, Name = "Delta Ltd", Email = "support.delta@gmail.com", Phone = null, Company = "Delta", CreatedDate = new DateTime(2024, 6, 4), LastContactDate = null, Status = CustomerStatus.Active, Priority = CustomerPriority.Medium, AssignedSalesRepId = null, Revenue = 75000 },
            new Customer { ID = 5, Name = "Epsilon GmbH", Email = "contact.epsilon@gmail.com", Phone = "+91 9456789012", Company = null, CreatedDate = new DateTime(2024, 6, 5), LastContactDate = new DateTime(2024, 6, 9), Status = CustomerStatus.Prospect, Priority = CustomerPriority.High, AssignedSalesRepId = 2, Revenue = 120000 }
        );
        // Seed ContactHistory
        modelBuilder.Entity<ContactHistory>().HasData(
            new ContactHistory { ContactHistoryID = 10, CustomerID = 1, ContactType = "Email", Notes = "Initial contact", ContactDate = new DateTime(2024, 6, 5), Outcome = "Interested", CreatedByUserId = 2, FollowUpDate = new DateTime(2025, 6, 1), Duration = 30, ContactMethod = ContactMethod.Email },
            new ContactHistory { ContactHistoryID = 20, CustomerID = 2, ContactType = "Phone", Notes = "Follow-up call", ContactDate = new DateTime(2024, 6, 6), Outcome = "No answer", CreatedByUserId = 3, FollowUpDate = new DateTime(2025, 6, 20), Duration = 15, ContactMethod = ContactMethod.Phone },
            new ContactHistory { ContactHistoryID = 30, CustomerID = 3, ContactType = "Meeting", Notes = "Demo presented", ContactDate = new DateTime(2024, 6, 7), Outcome = "Considering", CreatedByUserId = 5, FollowUpDate = null, Duration = 60, ContactMethod = ContactMethod.InPerson },
            new ContactHistory { ContactHistoryID = 40, CustomerID = 4, ContactType = "VideoCall", Notes = "Sent proposal", ContactDate = new DateTime(2024, 6, 8), Outcome = "Waiting", CreatedByUserId = 3, FollowUpDate = new DateTime(2025, 5, 1), Duration = 10, ContactMethod = ContactMethod.VideoCall },
            new ContactHistory { ContactHistoryID = 50, CustomerID = 5, ContactType = "Phone", Notes = "Negotiation", ContactDate = new DateTime(2024, 6, 9), Outcome = "Deal closed", CreatedByUserId = 2, FollowUpDate = new DateTime(2025, 7, 1), Duration = 45, ContactMethod = ContactMethod.Phone },
            new ContactHistory { ContactHistoryID = 60, CustomerID = 1, ContactType = "Email", Notes = "Follow-up overdue", ContactDate = new DateTime(2024, 7, 1), Outcome = "No response", CreatedByUserId = 1, FollowUpDate = new DateTime(2024, 7, 10), Duration = 5, ContactMethod = ContactMethod.Email },
            new ContactHistory { ContactHistoryID = 70, CustomerID = 2, ContactType = "InPerson", Notes = "On-site visit", ContactDate = new DateTime(2024, 8, 1), Outcome = "Demo scheduled", CreatedByUserId = 4, FollowUpDate = null, Duration = 90, ContactMethod = ContactMethod.InPerson },
            new ContactHistory { ContactHistoryID = 80, CustomerID = 3, ContactType = "VideoCall", Notes = "Remote support", ContactDate = new DateTime(2024, 9, 1), Outcome = "Resolved", CreatedByUserId = 5, FollowUpDate = new DateTime(2024, 9, 10), Duration = 20, ContactMethod = ContactMethod.VideoCall },
            new ContactHistory { ContactHistoryID = 90, CustomerID = 4, ContactType = "Phone", Notes = "Contract renewal", ContactDate = new DateTime(2024, 10, 1), Outcome = "Renewed", CreatedByUserId = 2, FollowUpDate = null, Duration = 25, ContactMethod = ContactMethod.Phone },
            new ContactHistory { ContactHistoryID = 100, CustomerID = 5, ContactType = "Email", Notes = "Feedback requested", ContactDate = new DateTime(2024, 11, 1), Outcome = "Positive", CreatedByUserId = 1, FollowUpDate = new DateTime(2024, 11, 10), Duration = 10, ContactMethod = ContactMethod.Email },
            new ContactHistory { ContactHistoryID = 110, CustomerID = 1, ContactType = "Phone", Notes = "Scheduled follow-up for testing", ContactDate = new DateTime(2025, 6, 15, 20, 0, 0), Outcome = "Pending", CreatedByUserId = 2, FollowUpDate = new DateTime(2025, 6, 15, 0, 0, 0), Duration = 10, ContactMethod = ContactMethod.Phone }
        );
    }
    public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<ContactHistory> ContactHistories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Notification> Notifications { get; set; }
}