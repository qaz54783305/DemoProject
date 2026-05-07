using DemoProject.Core.Data;
using DemoProject.Core.Models.Dto;
using DemoProject.Core.Models.Entities;

namespace DemoProject.Core.Services;

public class BudgetService : IBudgetService
{
    private readonly AppDbContext _db;

    private static readonly string[] AllowedStatuses = ["Draft", "Reviewing", "Approved"];

    public BudgetService(AppDbContext db) => _db = db;

    public List<Budget> GetAll() => _db.Budgets.ToList();

    public List<Budget> GetByBrand(string brand) =>
        _db.Budgets.Where(b => b.Brand == brand).ToList();

    public Budget Create(BudgetDto dto)
    {
        var budget = new Budget
        {
            Brand = dto.Brand, Channel = dto.Channel,
            Year = dto.Year, Month = dto.Month,
            BudgetAmount = dto.BudgetAmount, ActualAmount = dto.ActualAmount,
            Status = "Draft", UpdatedAt = DateTime.UtcNow
        };
        _db.Budgets.Add(budget);
        _db.SaveChanges();
        return budget;
    }

    public (Budget? Data, string? Error) Update(int id, BudgetDto dto)
    {
        var budget = _db.Budgets.Find(id);
        if (budget == null) return (null, "預算不存在");
        if (budget.Status == "Approved") return (null, "已核准的預算不可修改");

        budget.BudgetAmount = dto.BudgetAmount;
        budget.ActualAmount = dto.ActualAmount;
        budget.UpdatedAt = DateTime.UtcNow;
        _db.SaveChanges();
        return (budget, null);
    }

    public (Budget? Data, string? Error) UpdateStatus(int id, string status)
    {
        var budget = _db.Budgets.Find(id);
        if (budget == null) return (null, "預算不存在");
        if (!AllowedStatuses.Contains(status)) return (null, "無效的狀態，請使用 Draft / Reviewing / Approved");

        budget.Status = status;
        budget.UpdatedAt = DateTime.UtcNow;
        _db.SaveChanges();
        return (budget, null);
    }
}
