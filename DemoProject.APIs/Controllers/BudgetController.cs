using DemoProject.Core.Models;
using DemoProject.Core.Models.Dto;
using DemoProject.Core.Models.Entities;
using DemoProject.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BudgetController : ControllerBase
{
    private readonly IBudgetService _budgetService;

    public BudgetController(IBudgetService budgetService) => _budgetService = budgetService;

    /// <summary>取得所有預算</summary>
    [HttpGet]
    public IActionResult GetAll() =>
        Ok(ApiResponse<List<Budget>>.Ok(_budgetService.GetAll()));

    /// <summary>依品牌查詢預算</summary>
    [HttpGet("brand/{brand}")]
    public IActionResult GetByBrand(string brand) =>
        Ok(ApiResponse<List<Budget>>.Ok(_budgetService.GetByBrand(brand)));

    /// <summary>新增預算</summary>
    [HttpPost]
    public IActionResult Create([FromBody] BudgetDto dto) =>
        Ok(ApiResponse<Budget>.Ok(_budgetService.Create(dto), "新增成功"));

    /// <summary>更新預算金額（Approved 狀態不可修改）</summary>
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] BudgetDto dto)
    {
        var (data, error) = _budgetService.Update(id, dto);
        if (error == "預算不存在") return NotFound(ApiResponse<Budget>.Fail(error));
        if (error != null) return BadRequest(ApiResponse<Budget>.Fail(error));
        return Ok(ApiResponse<Budget>.Ok(data!, "更新成功"));
    }

    /// <summary>更新審核狀態（Draft → Reviewing → Approved）</summary>
    [HttpPatch("{id}/status")]
    public IActionResult UpdateStatus(int id, [FromBody] UpdateBudgetStatusDto dto)
    {
        var (data, error) = _budgetService.UpdateStatus(id, dto.Status);
        if (error == "預算不存在") return NotFound(ApiResponse<Budget>.Fail(error));
        if (error != null) return BadRequest(ApiResponse<Budget>.Fail(error));
        return Ok(ApiResponse<Budget>.Ok(data!, $"狀態已更新為 {dto.Status}"));
    }
}
