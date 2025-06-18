using MockCRM.Data;
using MockCRM.Models;

namespace MockCRM.Services;

public class HighPriorityCustomerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public HighPriorityCustomerBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                await notificationService.TriggerHighPriorityCustomerAlertsAsync();
            }
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}