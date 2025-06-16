using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MockCRM.Services;
using MockCRM.Models;

namespace MockCRM.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationTestController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly Data.CrmDbContext _context;

        public NotificationTestController(INotificationService notificationService, Data.CrmDbContext context)
        {
            _notificationService = notificationService;
            _context = context;
        }

        [HttpPost("test-send")]
        public async Task<IActionResult> TestSend([FromQuery] int userId, [FromQuery] string? message = null)
        {
            await _notificationService.DailyFollowUpCallsNotificationAsync(userId, message);
            var notification = _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .FirstOrDefault();
            return Ok(notification);
        }
    }
}
