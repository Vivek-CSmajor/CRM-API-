using Microsoft.EntityFrameworkCore;
using MockCRM.Data;
using MockCRM.Models;

namespace MockCRM.Services;

public interface INotificationService
{
    Task DailyFollowUpCallsNotificationAsync(int? userId, string message);
    Task<int> TriggerHighPriorityCustomerAlertsAsync();
}

public class NotificationService : INotificationService
{
    private readonly CrmDbContext _context;

    public NotificationService(CrmDbContext context)
    {
        _context = context;
    }

    //i didnt write this . i just copied it from the internet. i dont understand why its written like this
    // i just know what it does and slightly how it does it.
    public async Task DailyFollowUpCallsNotificationAsync(int? userId, string message)
    {
        // If message is null or empty, auto-generate follow-up list
        if (string.IsNullOrWhiteSpace(message) && userId.HasValue)
        {
            var today = DateTime.UtcNow.Date;
            var dueFollowups = _context.Set<ContactHistory>()
                .Where(ch => ch.CreatedByUserId == userId && ch.FollowUpDate != null && ch.FollowUpDate.Value.Date == today)
                .Join(_context.Customers, ch => ch.CustomerID, c => c.ID, (ch, c) => new { ch, c })
                .ToList();
            if (dueFollowups.Any())
            {
                message = "Calls due for follow-up:\n" + string.Join("\n", dueFollowups.Select(df => $"Customer: {df.c.Name}, FollowUp: {df.ch.FollowUpDate:yyyy-MM-dd}, Notes: {df.ch.Notes}"));
            }
            else
            {
                message = "No calls due for follow-up.";
            }
        }
        var notification = new Notification
        {
            UserId = userId,
            Message = message,
            CreatedAt = DateTime.UtcNow,
            isRead = false
        };
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        Console.WriteLine($"[Notification] User {userId} - {message} ");
    }

    public async Task<int> TriggerHighPriorityCustomerAlertsAsync()
    {
        var sevenDaysAgo = DateTime.Now.AddDays(-7);
        var neglectedCustomers = _context.Customers
            .Where(c => c.Priority == CustomerPriority.High && c.LastContactDate <= sevenDaysAgo)
            .ToList();
        foreach (var neglectedCustomer in neglectedCustomers)
        {
            var alert = $"High Priority customer {neglectedCustomer.Name} hasn't been contacted in the last 7 days";
            _context.Notifications.Add(new Notification
            {
                UserId = neglectedCustomer.AssignedSalesRepId,
                Message = alert,
                CreatedAt = DateTime.UtcNow,
                isRead = false
            });
        }
        await _context.SaveChangesAsync();
        return neglectedCustomers.Count;
    }
    
    public async Task<List<WeeklySummaryDto>> GetWeeklySummaryAsync()
    {
        var contactHistories = await _context.ContactHistories
            .Where(c => c.ContactDate >= DateTime.UtcNow.AddDays(-7))
            .ToListAsync();
        var summaries = contactHistories
            .Where(c=>c.ContactDate >= DateTime.UtcNow.AddDays(-7))
            .GroupBy(c=> c.CustomerID)
            .Select(g => new WeeklySummaryDto
            {
                CustomerId = g.Key,
                ActivityCounts = g.GroupBy(c => c.ContactType)
                    .ToDictionary(c => c.Key, c => c.Count())
            })
            .ToList();
        return summaries;
    }
    //instead of making a seperate file for this DTO . just write it here.
    public class WeeklySummaryDto
    {
        public int CustomerId { get; set; }
        public Dictionary<string,int> ActivityCounts { get; set; }
    }
}


