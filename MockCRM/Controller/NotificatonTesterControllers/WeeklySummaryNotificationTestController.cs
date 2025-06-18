using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MockCRM.Data;
using MockCRM.Services;

namespace MockCRM.Controller;
[Authorize]
[Microsoft.AspNetCore.Components.Route("api/[controller]")]
public class WeeklySummaryNotificationTestController : ControllerBase
{
    private readonly CrmDbContext _context;
    private readonly INotificationService _notificationService;

    public WeeklySummaryNotificationTestController(INotificationService notificationService,CrmDbContext context)
    {
        _notificationService = notificationService;
        _context = context;
    }

    [HttpGet("WeeklySummaryTest")]
    public async Task<IActionResult> WeeklySummaryTest([FromQuery] int userId)
    {
        var summaryService = new NotificationService(_context);
        var summaries = await summaryService.GetWeeklySummaryAsync();
        var userSummary = summaries
            .Where(c => c.CustomerId == userId)
            .ToList();
        return Ok(userSummary);
    }
}