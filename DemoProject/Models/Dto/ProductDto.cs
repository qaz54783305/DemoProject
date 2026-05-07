namespace DemoProject.Models.Dto;

public class ProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Stock { get; set; }
}
