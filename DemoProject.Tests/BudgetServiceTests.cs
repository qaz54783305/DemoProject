using DemoProject.Core.Data;
using DemoProject.Core.Models.Dto;
using DemoProject.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.Tests;

public class BudgetServiceTests
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
    public void Create_NewBudget_HasDraftStatus()
    {
        var service = new BudgetService(CreateDb());
        var dto = new BudgetDto { Brand = "TestBrand", Channel = "電商", Year = 2026, Month = 6, BudgetAmount = 100000, ActualAmount = 0 };

        var result = service.Create(dto);

        Assert.Equal("Draft", result.Status);
        Assert.Equal("TestBrand", result.Brand);
    }

    [Fact]
    public void Update_Fails_WhenBudgetIsApproved()
    {
        var service = new BudgetService(CreateDb());
        var dto = new BudgetDto { Brand = "LuxBag", Channel = "百貨", Year = 2026, Month = 1, BudgetAmount = 999999, ActualAmount = 0 };

        // Id=1 的預算狀態是 Approved
        var (data, error) = service.Update(1, dto);

        Assert.Null(data);
        Assert.Equal("已核准的預算不可修改", error);
    }

    [Fact]
    public void Update_Succeeds_WhenBudgetIsDraft()
    {
        var service = new BudgetService(CreateDb());
        var dto = new BudgetDto { Brand = "SportX", Channel = "電商", Year = 2026, Month = 2, BudgetAmount = 888888, ActualAmount = 0 };

        // Id=5 的預算狀態是 Draft
        var (data, error) = service.Update(5, dto);

        Assert.NotNull(data);
        Assert.Null(error);
        Assert.Equal(888888, data.BudgetAmount);
    }

    [Fact]
    public void UpdateStatus_Changes_Status()
    {
        var service = new BudgetService(CreateDb());

        // Id=5 是 Draft，改成 Reviewing
        var (data, error) = service.UpdateStatus(5, "Reviewing");

        Assert.NotNull(data);
        Assert.Null(error);
        Assert.Equal("Reviewing", data.Status);
    }

    [Fact]
    public void UpdateStatus_Fails_WithInvalidStatus()
    {
        var service = new BudgetService(CreateDb());

        var (data, error) = service.UpdateStatus(1, "InvalidStatus");

        Assert.Null(data);
        Assert.NotNull(error);
    }

    [Fact]
    public void UpdateStatus_Returns_Error_WhenNotFound()
    {
        var service = new BudgetService(CreateDb());

        var (data, error) = service.UpdateStatus(999, "Approved");

        Assert.Null(data);
        Assert.Equal("預算不存在", error);
    }

    [Fact]
    public void GetByBrand_Returns_FilteredResults()
    {
        var service = new BudgetService(CreateDb());

        var result = service.GetByBrand("LuxBag");

        Assert.True(result.Count > 0);
        Assert.All(result, b => Assert.Equal("LuxBag", b.Brand));
    }
}
