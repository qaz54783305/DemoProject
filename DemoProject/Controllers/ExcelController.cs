using DemoProject.Data;
using DemoProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoProject.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExcelController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ExcelExportService _excel;

    public ExcelController(AppDbContext db, ExcelExportService excel)
    {
        _db = db;
        _excel = excel;
    }

    /// <summary>匯出商品清單 Excel</summary>
    [HttpGet("products")]
    public IActionResult ExportProducts()
    {
        var bytes = _excel.ExportProducts(_db.Products.ToList());
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "商品清單.xlsx");
    }

    /// <summary>匯出預算報表 Excel（含狀態顏色標示）</summary>
    [HttpGet("budgets")]
    public IActionResult ExportBudgets()
    {
        var bytes = _excel.ExportBudgets(_db.Budgets.ToList());
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "預算報表.xlsx");
    }
}
