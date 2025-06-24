namespace MockCRM.Models;

public enum NotificationQueueStatus
{
    Pending,
    Sent,
    Failed,
    Delivered
}

public enum NotificationPriority
{
    Low,
    Medium,
    High
}
public class NotificationQueue
{
    public int Id { get; set; }
    public int CampaignId { get; set; }
    public NotificationCampaign Campaign { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public string Message { get; set; }
    public NotificationPriority Priority { get; set; }
    public NotificationQueueStatus Status { get; set; }
    public DateTime? LastTriedAt { get; set; }
}