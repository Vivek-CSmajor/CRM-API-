using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MockCRM.Models;

public enum CustomerStatus
{
    Active,
    Inactive,
    Prospect
}

public enum CustomerPriority
{
    High,
    Medium,
    Low
}

public enum ContactMethod
{
    Phone,
    Email,
    InPerson,
    VideoCall
}
public class Customer
{
    public CustomerStatus? Status { get; set; } = CustomerStatus.Active;
    public CustomerPriority? Priority { get; set; } = CustomerPriority.Medium;
    public int? Revenue { get; set; } //assuming revenue is whole numbers always
    public int? AssignedSalesRepId { get; set; }
    public User AssignedSalesRep { get; set; }
    
    public int ID { get; set; }
    [Required(ErrorMessage = "Name is Req")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Email is Req")]
    [EmailAddress(ErrorMessage = "invalid email format")]
    public string Email { get; set; }
    
    public string Phone { get; set; }
    public string Company { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastContactDate { get; set; }
    public List<ContactHistory>? ContactHistories { get; set; }
}

public class ContactHistory
{
    [JsonIgnore]
    public Customer? Customer { get; set; } //this is added for cascade delete configuration
    public int ContactHistoryID { get; set; }
    public int CustomerID { get; set; }
    public string ContactType { get; set; }
    public string Notes { get; set; }
    public DateTime ContactDate { get; set; }
    public string Outcome { get; set; }
    public int Duration { get; set; } //in minutes for calls
    public DateTime? FollowUpDate { get; set; }//for scheduling next time of contact for the customers-salesRep
    public int? CreatedByUserId { get; set; } //to track who created this contact history becomes the FK to below
    public User CreatedByUser { get; set; } //to track who created this contact history
    public ContactMethod ContactMethod { get; set; }
}
