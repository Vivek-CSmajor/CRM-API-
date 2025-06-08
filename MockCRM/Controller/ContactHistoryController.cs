using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockCRM.Data;
using MockCRM.Models;

namespace MockCRM.Controller;
[Authorize]
[Route("api/customers/{customerId}/contacts")]
[ApiController]
public class ContactHistoryController : ControllerBase
{
    private readonly CrmDbContext _context;

    public ContactHistoryController(CrmDbContext context)
    {
        _context = context;
    }
    private async Task<bool> CustomerExists(int customerId)
    {
        return await _context.Customers.AnyAsync((c => c.ID == customerId));
    }

    [HttpGet]
    public async Task<IActionResult> GetContacts(int customerId)
    {
        if(!await CustomerExists(customerId))
            return NotFound("customer nhi mila");
        var contacts = await _context.ContactHistories
            .Where(ch => ch.CustomerID == customerId)
            .OrderByDescending(ch => ch.ContactDate)
            .ToListAsync();
        return Ok(contacts);
    }

    [HttpPost] //for adding new contact history record for a customer
    public async Task<IActionResult> AddContacts(int customerId, [FromBody] ContactHistory contactData)
    {
     if(!await CustomerExists(customerId))
         return NotFound("customer nhi mila");
     if(string.IsNullOrWhiteSpace(contactData.ContactType) || string.IsNullOrWhiteSpace(contactData.Outcome))
         return BadRequest("CONtact type and/or outcome is null/whitespace");
     var newContact = new ContactHistory()
     {
         CustomerID = customerId,
         ContactType = contactData.ContactType,
         Notes = contactData.Notes,
         Outcome = contactData.Outcome,
         ContactDate = DateTime.UtcNow
     };
     _context.ContactHistories.Add(newContact);
     await _context.SaveChangesAsync();

     var customer = await _context.Customers.FindAsync(customerId);
     customer.LastContactDate = newContact.ContactDate;
     await _context.SaveChangesAsync();

     return CreatedAtAction(nameof(GetContacts), new { customerId = customerId }, newContact);
    //this above line tells the client "new contact created
    //. Here's the data for it and here's the URL where you can.
    //get all contacts for this customer"
    }
}