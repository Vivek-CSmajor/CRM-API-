using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockCRM.Data;

namespace MockCRM.Controller;
[Authorize]
[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly CrmDbContext _context;

    public AnalyticsController(CrmDbContext context)
    {
        _context = context;
    }

    [HttpGet("contacts-today")]
    public async Task<IActionResult> GetContactsCreatedToday()
    {
        var contactsCreatedToday = await _context.ContactHistories
            .Where(c => EF.Functions.DateDiffDay(c.ContactDate, DateTime.UtcNow.Date) == 0)
            .ToListAsync();
        return Ok(contactsCreatedToday);
    }
}