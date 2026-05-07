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
public class ProductController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductController(AppDbContext db) => _db = db;

    /// <summary>取得所有商品</summary>
    [HttpGet]
    public IActionResult GetAll() =>
        Ok(ApiResponse<List<Product>>.Ok(_db.Products.ToList()));

    /// <summary>依 ID 取得商品</summary>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = _db.Products.Find(id);
        return product == null
            ? NotFound(ApiResponse<Product>.Fail("商品不存在"))
            : Ok(ApiResponse<Product>.Ok(product));
    }

    /// <summary>新增商品</summary>
    [HttpPost]
    public IActionResult Create([FromBody] ProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name, Brand = dto.Brand,
            Category = dto.Category, UnitPrice = dto.UnitPrice,
            Stock = dto.Stock, CreatedAt = DateTime.UtcNow
        };
        _db.Products.Add(product);
        _db.SaveChanges();
        return Ok(ApiResponse<Product>.Ok(product, "新增成功"));
    }

    /// <summary>更新商品</summary>
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ProductDto dto)
    {
        var product = _db.Products.Find(id);
        if (product == null) return NotFound(ApiResponse<Product>.Fail("商品不存在"));

        product.Name = dto.Name; product.Brand = dto.Brand;
        product.Category = dto.Category; product.UnitPrice = dto.UnitPrice;
        product.Stock = dto.Stock;
        _db.SaveChanges();
        return Ok(ApiResponse<Product>.Ok(product, "更新成功"));
    }

    /// <summary>刪除商品（僅 Admin）</summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var product = _db.Products.Find(id);
        if (product == null) return NotFound(ApiResponse<Product>.Fail("商品不存在"));

        _db.Products.Remove(product);
        _db.SaveChanges();
        return Ok(ApiResponse<string>.Ok("已刪除"));
    }
}
