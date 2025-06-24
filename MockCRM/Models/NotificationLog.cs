namespace MockCRM.Models;

public class NotificationLog
{
    public int Id { get; set; }
    public int CampaignId { get; set; }
    public NotificationCampaign Campaign { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public DateTime SentTime { get; set; }
    public bool isSuccessful { get; set; }
    public string ErrorMessage { get; set; }
}