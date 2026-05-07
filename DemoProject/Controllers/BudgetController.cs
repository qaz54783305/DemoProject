using DemoProject.Data;
using DemoProject.Models;
using DemoProject.Models.Dto;
using DemoProject.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BudgetController : ControllerBase
{
    private readonly AppDbContext _db;

    public BudgetController(AppDbContext db) => _db = db;

    /// <summary>取得所有預算</summary>
    [HttpGet]
    public IActionResult GetAll() =>
        Ok(ApiResponse<List<Budget>>.Ok(_db.Budgets.ToList()));

    /// <summary>依品牌查詢預算</summary>
    [HttpGet("brand/{brand}")]
    public IActionResult GetByBrand(string brand)
    {
        var list = _db.Budgets.Where(b => b.Brand == brand).ToList();
        return Ok(ApiResponse<List<Budget>>.Ok(list));
    }

    /// <summary>新增預算</summary>
    [HttpPost]
    public IActionResult Create([FromBody] BudgetDto dto)
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
        return Ok(ApiResponse<Budget>.Ok(budget, "新增成功"));
    }

    /// <summary>更新預算金額（Approved 狀態不可修改）</summary>
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] BudgetDto dto)
    {
        var budget = _db.Budgets.Find(id);
        if (budget == null) return NotFound(ApiResponse<Budget>.Fail("預算不存在"));
        if (budget.Status == "Approved")
            return BadRequest(ApiResponse<Budget>.Fail("已核准的預算不可修改"));

        budget.BudgetAmount = dto.BudgetAmount;
        budget.ActualAmount = dto.ActualAmount;
        budget.UpdatedAt = DateTime.UtcNow;
        _db.SaveChanges();
        return Ok(ApiResponse<Budget>.Ok(budget, "更新成功"));
    }

    /// <summary>更新審核狀態（Draft → Reviewing → Approved）</summary>
    [HttpPatch("{id}/status")]
    public IActionResult UpdateStatus(int id, [FromBody] UpdateBudgetStatusDto dto)
    {
        var budget = _db.Budgets.Find(id);
        if (budget == null) return NotFound(ApiResponse<Budget>.Fail("預算不存在"));

        var allowed = new[] { "Draft", "Reviewing", "Approved" };
        if (!allowed.Contains(dto.Status))
            return BadRequest(ApiResponse<Budget>.Fail("無效的狀態，請使用 Draft / Reviewing / Approved"));

        budget.Status = dto.Status;
        budget.UpdatedAt = DateTime.UtcNow;
        _db.SaveChanges();
        return Ok(ApiResponse<Budget>.Ok(budget, $"狀態已更新為 {dto.Status}"));
    }
}
