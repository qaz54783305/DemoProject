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
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService) => _productService = productService;

    /// <summary>取得所有商品</summary>
    [HttpGet]
    public IActionResult GetAll() =>
        Ok(ApiResponse<List<Product>>.Ok(_productService.GetAll()));

    /// <summary>依 ID 取得商品</summary>
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = _productService.GetById(id);
        return product == null
            ? NotFound(ApiResponse<Product>.Fail("商品不存在"))
            : Ok(ApiResponse<Product>.Ok(product));
    }

    /// <summary>新增商品</summary>
    [HttpPost]
    public IActionResult Create([FromBody] ProductDto dto) =>
        Ok(ApiResponse<Product>.Ok(_productService.Create(dto), "新增成功"));

    /// <summary>更新商品</summary>
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] ProductDto dto)
    {
        var product = _productService.Update(id, dto);
        return product == null
            ? NotFound(ApiResponse<Product>.Fail("商品不存在"))
            : Ok(ApiResponse<Product>.Ok(product, "更新成功"));
    }

    /// <summary>刪除商品（僅 Admin）</summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        return _productService.Delete(id)
            ? Ok(ApiResponse<string>.Ok("已刪除"))
            : NotFound(ApiResponse<string>.Fail("商品不存在"));
    }
}
