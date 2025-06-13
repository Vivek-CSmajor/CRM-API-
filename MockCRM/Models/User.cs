using System.ComponentModel.DataAnnotations;

namespace MockCRM.Models;
public enum Role
{
    Admin,
    Manager,
    Salesman,
    DeveloperTester
}
public class User
{
    public List<Customer> AssignedCustomers { get; set; }
    public List<ContactHistory> ContactHistoriesCreatedbyUser { get; set; }
    public int  Id { get; set; }
    public string Username { get; set; }
    [Required(ErrorMessage = "Email is Req")]
    [EmailAddress(ErrorMessage = "email format is invalid")]
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public Role Role { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public User()
    {
        CreatedDate = DateTime.Now;
    }
}

public class RegisterRequest
{
    [Required(ErrorMessage = "Email is Req")]
    [EmailAddress(ErrorMessage = "email format is invalid")]
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    [Required]
    public Role Role { get; set; }
}

public class LoginRequest
{
    public string EmailOrUsername { get; set; }
    public string Password { get; set; }
}