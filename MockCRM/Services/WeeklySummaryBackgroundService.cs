namespace MockCRM.Services;

public class WeeklySummaryBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public WeeklySummaryBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRun = now.Date.AddDays(((int)DayOfWeek.Monday -(int)now.DayOfWeek + 7) % 7).AddHours(8);
            if (nextRun < now)
                nextRun = nextRun.AddDays(7);
            var delay = nextRun - now;
            await Task.Delay(delay, stoppingToken);
            using (var scope = _serviceProvider.CreateScope())
            {
                var summaryService = scope.ServiceProvider.GetRequiredService<WeeklySummaryService>();
                var summaries = await summaryService.GetWeeklySummaryAsync();
                foreach (var summary in summaries)
                {
                    Console.WriteLine($"Customer {summary.CustomerId} : {string.Join(" ,", summary.ActivityCounts.Select(x => $"{x.Key} : {x.Value}"))}");
                }
            }
        }
    }
}