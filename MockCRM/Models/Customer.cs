using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MockCRM.Models;

public class Customer
{
    
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
    public List<ContactHistory> ContactHistories { get; set; }
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
}
