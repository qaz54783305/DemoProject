namespace DemoProject.Models.Entities;

public class Budget
{
    public int Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal BudgetAmount { get; set; }
    public decimal ActualAmount { get; set; }
    /// <summary>Draft / Reviewing / Approved</summary>
    public string Status { get; set; } = "Draft";
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
