using System.ComponentModel.DataAnnotations;

namespace MockCRM.Models;

public enum NotificationType
{
    Email,
    SMS,
    Push,
    InApp
}
public class NotificationTemplate
{
    public int Id { get; set; } 
    [Required]
    public NotificationType Type { get; set; }
    [Required]
    public string Content { get; set; }
    
    public string Variables { get; set; }
    public bool isActive { get; set; } = true;
}
