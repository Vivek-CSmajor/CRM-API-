using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockCRM.Data;
using MockCRM.Models;

namespace MockCRM.Controller;
[Authorize]
[Route("api/campaigns/[controller]")]
public class NotificationCampaignsController : ControllerBase
{
    private readonly CrmDbContext _context;

    public NotificationCampaignsController(CrmDbContext context)
    {
        _context = context; 
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCampaigns()
    {
        var campaigns = await _context.NotificationCampaigns
            .ToListAsync();
        //i m ok with returning all properties therefore no select LINQ query used.
        return Ok(campaigns);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCampaignById(int id)
    {
        var campaign = await _context.NotificationCampaigns
            .FirstOrDefaultAsync(c => c.Id == id);
        return Ok( campaign);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCampaign(int id, [FromBody] NotificationCampaign updatedCampaign)
    {
        var campaign = await _context.NotificationCampaigns.FirstOrDefaultAsync(c => c.Id == id);
        if (campaign == null)
            return NotFound();
        if(campaign.Status == CampaignStatus.Completed || campaign.Status == CampaignStatus.Running)
            return BadRequest("cannot update a completed or running campaign");
        campaign.Name = updatedCampaign.Name;
        campaign.TemplateId = updatedCampaign.TemplateId;
        campaign.TargetCustomerIds = updatedCampaign.TargetCustomerIds;
        campaign.ScheduledTime = updatedCampaign.ScheduledTime;
        campaign.Status = updatedCampaign.Status;
        await _context.SaveChangesAsync();
        return Ok(campaign);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCampaign(int id)
    {
        var campaign = await _context.NotificationCampaigns.FirstOrDefaultAsync(c => c.Id == id);
        if (campaign == null)
            return NotFound();

        if (campaign.Status == CampaignStatus.Completed || campaign.Status == CampaignStatus.Running)
            return BadRequest("canot delete a completed/running campaign");

        _context.NotificationCampaigns.Remove(campaign);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/start")]
    public async Task<IActionResult> StartCampaign(int id)
    {
        var campaign = await _context.NotificationCampaigns
            .FirstOrDefaultAsync(c => c.Id == id);
        if(campaign == null)
            return NotFound();
        if(campaign.Status == CampaignStatus.Running || campaign.Status == CampaignStatus.Completed)
            return BadRequest("Campaign is already running or completed");
        campaign.Status = CampaignStatus.Running;
        campaign.ScheduledTime = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return Ok(campaign);
    }
    
}