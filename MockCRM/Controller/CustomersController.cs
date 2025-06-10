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

        [HttpGet("search")]
        //since this can give All customers is no filter provided.ITs a viable 
        //replaceemnt for GetAllCustomers. Still keep both for clarity.
        public async Task<IActionResult> SearchCustomersByName_Company([FromQuery] string? name,
            [FromQuery] string? company)
        {
            var query = _context.Customers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.Name.Contains(name));
            if (!string.IsNullOrWhiteSpace(company))
                query = query.Where(c => c.Company.Contains(company));
            var customers = await query.ToListAsync();
            return Ok(customers);
        }

        [HttpGet("by-status/{status}")]
        public async Task<IActionResult> GetCustomersbyStatus([FromRoute] string status)
        {
            if (!Enum.TryParse<CustomerStatus>(status, true, out var parsedStatus))
                return BadRequest("invalid status");
            var customers = await _context.Customers
                .Where(c => c.Status == parsedStatus)
                .ToListAsync();
            return Ok(customers);
        }

        [HttpGet("by-priority/{priority}")]
        public async Task<IActionResult> GetCustomersByPriority([FromRoute] string priority)
        {
            //this if condition works like : if parsing fails returns BadRequest. 
            //if parsing succeeds, it will return a new var parsedPriority that can be used querying later.
            if (!Enum.TryParse<CustomerPriority>(priority, true, out var parsedPriority))
                return BadRequest("invalid priority");
            var customers = await _context.Customers
                .Where(c => c.Priority == parsedPriority)
                .ToListAsync();
            return Ok(customers);
        }

        [HttpGet("assigned-to/{userId}")]
        public async Task<IActionResult> GetCUstomersAssignedToUser([FromRoute] int userId)
        {
            if (!await _context.Users.AnyAsync(c => c.Id == userId))
                return BadRequest("User does not exist. Invalid Id given by u");
            var customers = await _context.Customers
                .Where(c => c.AssignedSalesRepId == userId)
                .ToListAsync();
            //below statement is written this way beacuse
            //ToListAsync() returns empty list if no customers found. IT doesnt return null.
            if (customers.Count == 0)
                return Ok(new List<Customer>());
            return Ok(customers);
        }

        [HttpPost("bulk-import")]
        // This made me realise i need to create a Dto as the request i'll give here may not include all the properties. 
        // Since when importing customers, i may have not done all the things like phone, company etc.
        public async Task<IActionResult> BulkImportCustomers([FromBody] List<CustomerImportDto> customers)
        {
            // Initialize collections for processing results and tracking duplicate emails
            var results = new List<object>();
            var emailsInRequest = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Process each customer in the import request
            foreach (var dto in customers)
            {
                // First. we perform all basic validations and checks .
                // Check if name and email are provided (both are required fields)
                if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email))
                {
                    results.Add(new { customer = dto, success = false, reason = "name & email are req" });
                    continue;
                }

                // Below since emailsInRequest is a hashset it will not allow duplications 
                // Therefore when we add already existing email it returns false so we can use the if condition like this
                // Btw below we are checking if email is already in the REQUEST
                if (!emailsInRequest.Add(dto.Email))
                {
                    results.Add(new { customer = dto, success = false, reason = "Email already exists" });
                    continue;
                }

                // Now we check if email already exists in database
                if (await _context.Customers.AnyAsync(c => c.Email == dto.Email))
                {
                    results.Add(new { customer = dto, success = false, reason = "Email already in database" });
                    continue;
                }

                // Validate Status enum - ensure provided status is valid
                if (!Enum.TryParse<CustomerStatus>(dto.Status, true, out var status))
                {
                    results.Add(new { customer = dto, success = false, reason = "Invalid status" });
                    continue;
                }

                // Validate Priority enum - ensure provided priority is valid
                if (!Enum.TryParse<CustomerPriority>(dto.Priority, true, out var priority))
                {
                    results.Add(new { customer = dto, success = false, reason = "Invalid priority" });
                    continue;
                }

                // This is to check if AssignedSalesRepId is provided and if it exists in the database
                // These things are for null coalescing - in case nothing is provided for certain things 
                // like AssignedSalesRepId, or revenue etc, we can just set them to null or 0
                if (dto.AssignedSalesRepId.HasValue)
                {
                    var userExists = await _context.Users.AnyAsync(u => u.Id == dto.AssignedSalesRepId.Value);
                    if (!userExists)
                    {
                        results.Add(new { customer = dto, success = false, reason = "Assigned sales rep does not exist" });
                        continue;
                    }
                }
                // ALl validations Done. Now create Customer.
                // Create new customer entity from validated DTO
                var customer = new Customer
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    Company = dto.Company,
                    Status = status,
                    Priority = priority,
                    AssignedSalesRepId = dto.AssignedSalesRepId,  // Set directly, allow null
                    Revenue = dto.Revenue ?? 0,                   // Use dto.Revenue if provided, else 0
                    CreatedDate = DateTime.UtcNow
                };

                // Add customer to context and mark as successful
                _context.Customers.Add(customer);
                results.Add(new { customer = dto, success = true });
            }

            // Save all valid customers to database in a single transaction
            await _context.SaveChangesAsync();
            
            // Return results showing success/failure for each customer
            return Ok(results);
        }
    }
}
