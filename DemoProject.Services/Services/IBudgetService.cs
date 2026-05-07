using DemoProject.Core.Models.Dto;
using DemoProject.Core.Models.Entities;

namespace DemoProject.Core.Services;

public interface IBudgetService
{
    List<Budget> GetAll();
    List<Budget> GetByBrand(string brand);
    Budget Create(BudgetDto dto);
    (Budget? Data, string? Error) Update(int id, BudgetDto dto);
    (Budget? Data, string? Error) UpdateStatus(int id, string status);
}
