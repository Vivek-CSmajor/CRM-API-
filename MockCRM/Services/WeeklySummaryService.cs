using Microsoft.EntityFrameworkCore;
using MockCRM.Data;

namespace MockCRM.Services;
/*
✅ 2. Weekly Summary of Customer Activity
Goal: Summarize contact activity for the past 7 days.
How:
In same or a separate background service:
Once a week (e.g., every Monday 8 AM):
Get contact records created in the past 7 days
Group by CustomerId or CreatedByUserId
Count types (calls, emails, meetings, etc.)
Output summary per user or customer
*/


public class WeeklySummaryService
{
    private readonly CrmDbContext _context;

    public WeeklySummaryService(CrmDbContext context)
    {
        _context = context;
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
}

//instead of making a seperate file for this DTO . just write it here.
public class WeeklySummaryDto
{
    public int CustomerId { get; set; }
    public Dictionary<string,int> ActivityCounts { get; set; }
}