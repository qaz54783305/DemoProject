using DemoProject.Core.Models.Entities;

namespace DemoProject.Core.Services;

public interface IExcelExportService
{
    byte[] ExportProducts(List<Product> products);
    byte[] ExportBudgets(List<Budget> budgets);
}
