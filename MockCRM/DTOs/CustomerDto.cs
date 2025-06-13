using System.ComponentModel.DataAnnotations;

namespace MockCRM.Models;

public class CustomerDto
{
    public int? DaysSinceLastContact { get; set; }
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
    
    [RegularExpression(@"^\+91\s?[6-9]\d{9}$", 
        ErrorMessage = "invalid mobile number format, must be +91 followed by 10 digit phone number")]
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastContactDate { get; set; }
    public List<ContactHistory>? ContactHistories { get; set; }
}