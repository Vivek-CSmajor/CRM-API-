namespace MockCRM.Models;

public class Notification
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool isRead { get; set; }
}