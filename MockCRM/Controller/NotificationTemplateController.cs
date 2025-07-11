using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockCRM.Data;
using MockCRM.Models;

namespace MockCRM.Controller;

[Authorize]
[ApiController]
[Route("api/templates")]
public class NotificationTemplateController : ControllerBase
{
    private readonly CrmDbContext _context;

    public NotificationTemplateController(CrmDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTemplates()
    {
        var template = await _context.NotificationTemplates
            .ToListAsync();
        if (template.Count == 0) return NotFound("There are no templates to show yuh");
        return Ok(template);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTemplateById(int id)
    {
        var template = await _context.NotificationTemplates
            .FirstOrDefaultAsync(c => c.Id == id);
        if (template == null) return NotFound($"Template with ID = {id} not found");
        return Ok(template);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTemplate([FromBody] NotificationTemplate template)
    {
        if (!Enum.IsDefined(typeof(NotificationType), template.Type) || string.IsNullOrWhiteSpace(template.Content))
        {
            return BadRequest("Template Type and Content are required");
        }

        _context.NotificationTemplates.Add(template);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(CreateTemplate), new { id = template.Id }, template);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTemplate(int id, [FromBody] NotificationTemplate updatedTemplate)
    {
        var template = await _context.NotificationTemplates.FindAsync(id);
        if (template == null)
            return NotFound("template not found");
        if (!Enum.IsDefined(typeof(NotificationType), updatedTemplate.Type) ||
            string.IsNullOrWhiteSpace(updatedTemplate.Content))
        {
            return BadRequest("Template type and content are required");
        }

        template.isActive = updatedTemplate.isActive;
        template.Type = updatedTemplate.Type;
        template.Content = updatedTemplate.Content;
        template.Category = updatedTemplate.Category;
        await _context.SaveChangesAsync();
        return Ok(template);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTemplate(int id)
    {
        var template = await _context.NotificationTemplates.FindAsync(id);
        if (template == null) return NotFound("template not found");
        _context.NotificationTemplates.Remove(template);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("by-type/{type}")]
    public async Task<IActionResult> GetTemplatesByType(string type)
    {
        if (!Enum.TryParse<NotificationType>(type, true, out var notificationType))
            return BadRequest("Invalid notification type");

        var templates = await _context.NotificationTemplates
            .Where(t => t.Type == notificationType)
            .ToListAsync();

        if (templates.Count == 0)
            return NotFound($"No templates found for type = {type}");

        return Ok(templates);
    }

    //the following is for Template Variable system to dynamically personalize msgs
    private static readonly HashSet<string> SupportedVariables = new()
    {
        "CustomerName", "Company", "Email", "Phone"
    };

    private IEnumerable<string> ExtractVariables(string template)
    {
        var matches = Regex.Matches(template, @"\{\{(\w+)\}\}");
        foreach (Match match in matches)
            yield return match.Groups[1].Value;
    }

    //what does out do here?
    //its an output parameter that allows the function to return more than one values.
    private bool AreVariablesValid(string template, out List<string> invalidVariables)
    {
        var variables = ExtractVariables(template).Distinct();
        invalidVariables = variables.Where(v => !SupportedVariables.Contains(v)).ToList();
        return !invalidVariables.Any(); //! because we want to return true if there are no invalid variables
    }

    private string ReplaceVariables(string template, Dictionary<string, string> data)
    {
        return Regex.Replace(template, @"\{\{(\w+)\}\}", match =>
        {
            //use 1 inside Groups[1] to get just the value inside {{ }}. if used [0] it will give entire match like {{Name}}
            var key = match.Groups[1].Value;
            return data.TryGetValue(key, out var value) ? value : match.Value;
        });
    }

    //Provide an endpoint where a user can send a template and a customer ID,
    //and get back the template with variables replaced by that customer's real data.
    [HttpGet("preview/{customerId}")]
    public async Task<IActionResult> PreviewTemplateByCustomer(int customerId, [FromQuery] string template)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null) return NotFound("customer not found");
        var data = new Dictionary<string, string>
        {
            ["CustomerName"] = customer.Name,
            ["Company"] = customer.Company,
            ["Email"] = customer.Email,
            ["Phone"] = customer.Phone
        };
        var invalidVariables = ExtractVariables(template)
            .Where(v => !SupportedVariables.Contains(v))
            .ToList();
        if (invalidVariables.Any())
            return BadRequest($"Unsupported Variabels bitches : {string.Join(" ,", invalidVariables)}");
        var replaced = ReplaceVariables(template, data);
        return Ok(replaced);
    }
    
    //to get list of all templates by category
    [HttpGet("by-category/{category}")]
    public async Task<IActionResult> GetTemplatesByCategory(string category)
    {
        if (!Enum.TryParse<TemplateCategory>(category, true, out var parsedCategory))
            return BadRequest("Invalid Category");
        var templates = await _context.NotificationTemplates
            .Where(c => c.Category == parsedCategory)
            .ToListAsync();
        if (templates.Count == 0)
            return NotFound("No tempalte found for the given category dumbass");
        return Ok(templates);
    }
}