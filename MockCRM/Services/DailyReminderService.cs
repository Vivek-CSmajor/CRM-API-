using Microsoft.EntityFrameworkCore;
using MockCRM.Data;

namespace MockCRM.Services;

public class DailyReminderService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public DailyReminderService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    // i didnt write this . i just copied it from the internet. i dont understand why its written like this 
    // i just know what it does and slightly how it does it.
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<CrmDbContext>();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                var today = DateTime.UtcNow.Date;
                var dueContactsToday = await db.ContactHistories
                    .Where(c => c.FollowUpDate == today)
                    .GroupBy(c => c.CreatedByUserId)
                    .ToListAsync();
                foreach (var group in dueContactsToday)
                {
                    var userId = group.Key;
                    // Let NotificationService handle message generation
                    await notificationService.DailyFollowUpCallsNotificationAsync(userId, null);
                }
            }
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}