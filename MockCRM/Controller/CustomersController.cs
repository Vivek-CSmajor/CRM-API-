using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockCRM.Data;
using MockCRM.Models;

namespace MockCRM.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly CrmDbContext _context;

        public CustomersController(CrmDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name) || string.IsNullOrWhiteSpace(customer.Email))
            {
                return BadRequest("Name & Email are required bitch.");
            }

            if (_context.Customers.Any(c => c.Email == customer.Email))
            {
                return BadRequest("Email already exists.");
            }

            customer.CreatedDate = DateTime.UtcNow;
            //Save this to database
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(CreateCustomer), new { id = customer.ID }, customer);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var customers = await _context.Customers
                    .Include(c => c.ContactHistories)
                    .ToListAsync();
                return Ok(customers);
            }
            catch
            {
                return StatusCode(500, "error occured while doing GetAllCustomers ");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.ContactHistories)
                .FirstOrDefaultAsync(c => c.ID == id);
            if (customer == null)
                return NotFound("Customer not found . maybe deleted");
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();
            if (string.IsNullOrWhiteSpace(updatedCustomer.Name) || string.IsNullOrWhiteSpace(updatedCustomer.Email))
                return BadRequest("NAME EMAIL ARE REQUIRED.");

            customer.Name = updatedCustomer.Name;
            customer.Email = updatedCustomer.Email;
            customer.Phone = updatedCustomer.Phone;
            customer.Company = updatedCustomer.Company;
            customer.LastContactDate = updatedCustomer.LastContactDate;
            
            await _context.SaveChangesAsync();
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
    }
}
