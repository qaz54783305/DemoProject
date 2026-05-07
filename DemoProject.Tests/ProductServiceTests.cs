using DemoProject.Core.Data;
using DemoProject.Core.Models.Dto;
using DemoProject.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.Tests;

public class ProductServiceTests
{
    private static AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var db = new AppDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    [Fact]
    public void GetAll_Returns_All_SeededProducts()
    {
        var service = new ProductService(CreateDb());
        var result = service.GetAll();
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public void GetById_Returns_CorrectProduct()
    {
        var service = new ProductService(CreateDb());
        var result = service.GetById(1);
        Assert.NotNull(result);
        Assert.Equal("經典皮革手提包", result.Name);
    }

    [Fact]
    public void GetById_Returns_Null_WhenNotFound()
    {
        var service = new ProductService(CreateDb());
        var result = service.GetById(999);
        Assert.Null(result);
    }

    [Fact]
    public void Create_AddsProduct_AndReturnsIt()
    {
        var db = CreateDb();
        var service = new ProductService(db);
        var dto = new ProductDto { Name = "測試商品", Brand = "TestBrand", Category = "測試", UnitPrice = 1000, Stock = 10 };

        var result = service.Create(dto);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("測試商品", result.Name);
        Assert.Equal(6, service.GetAll().Count);
    }

    [Fact]
    public void Update_UpdatesProduct_WhenExists()
    {
        var db = CreateDb();
        var service = new ProductService(db);
        var dto = new ProductDto { Name = "更新後商品", Brand = "NewBrand", Category = "新類別", UnitPrice = 9999, Stock = 1 };

        var result = service.Update(1, dto);

        Assert.NotNull(result);
        Assert.Equal("更新後商品", result.Name);
        Assert.Equal(9999, result.UnitPrice);
    }

    [Fact]
    public void Update_Returns_Null_WhenNotFound()
    {
        var service = new ProductService(CreateDb());
        var dto = new ProductDto { Name = "X", Brand = "X", Category = "X", UnitPrice = 1, Stock = 1 };
        var result = service.Update(999, dto);
        Assert.Null(result);
    }

    [Fact]
    public void Delete_RemovesProduct_WhenExists()
    {
        var db = CreateDb();
        var service = new ProductService(db);

        var success = service.Delete(1);

        Assert.True(success);
        Assert.Equal(4, service.GetAll().Count);
    }

    [Fact]
    public void Delete_Returns_False_WhenNotFound()
    {
        var service = new ProductService(CreateDb());
        var result = service.Delete(999);
        Assert.False(result);
    }
}
