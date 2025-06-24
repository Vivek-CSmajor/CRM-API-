using System.ComponentModel.DataAnnotations;

namespace MockCRM.Models;

public enum CampaignStatus
{
    Draft,
    Scheduled,
    Running,
    Completed,
    Cancelled
}
public class NotificationCampaign
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    
    public int TemplateId { get; set; }
    public NotificationTemplate Template { get; set; }
    public string TargetCustomerIds { get; set; } // seperated by commas
    public DateTime ScheduledTime { get; set; }
    public string Status { get; set; } // scheduled , sent, failed
}