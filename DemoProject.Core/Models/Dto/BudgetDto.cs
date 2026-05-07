namespace DemoProject.Core.Models.Dto;

public class BudgetDto
{
    public string Brand { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal BudgetAmount { get; set; }
    public decimal ActualAmount { get; set; }
}

public class UpdateBudgetStatusDto
{
    /// <summary>Draft / Reviewing / Approved</summary>
    public string Status { get; set; } = string.Empty;
}
