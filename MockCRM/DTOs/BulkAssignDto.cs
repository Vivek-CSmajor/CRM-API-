namespace MockCRM.Models;

public class BulkAssignDto
{
    public List<int> CustomerIds { get; set; }
    public int AssignedSalesRepId { get; set; }
}