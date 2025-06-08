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
    public int  Id { get; set; }
    public string Username { get; set; }
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