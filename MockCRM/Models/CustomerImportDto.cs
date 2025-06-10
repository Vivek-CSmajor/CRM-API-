namespace MockCRM.Models;

public class CustomerImportDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public int? Revenue { get; set; } // assuming revenue is a whole number
    public int? AssignedSalesRepId { get; set; }
}