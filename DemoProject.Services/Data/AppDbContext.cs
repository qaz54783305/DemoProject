using DemoProject.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Budget> Budgets => Set<Budget>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", PasswordHash = "6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=", Role = "Admin", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new User { Id = 2, Username = "user1", PasswordHash = "PnwZV2SIhigW8TtRLKzz5LqX3ZckPqC9airRZC2GunI=", Role = "User",  CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "經典皮革手提包", Brand = "LuxBag",      Category = "包包", UnitPrice = 8500, Stock = 50,  CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 2, Name = "休閒運動鞋",     Brand = "SportX",     Category = "鞋類", UnitPrice = 2800, Stock = 120, CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 3, Name = "限量聯名帽T",    Brand = "StreetCo",   Category = "上衣", UnitPrice = 1980, Stock = 80,  CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 4, Name = "修身牛仔褲",     Brand = "DenimLab",   Category = "下著", UnitPrice = 2200, Stock = 60,  CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 5, Name = "防水登山背包",   Brand = "OutdoorPro", Category = "包包", UnitPrice = 3500, Stock = 35,  CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<Budget>().HasData(
            new Budget { Id = 1, Brand = "LuxBag", Channel = "百貨", Year = 2026, Month = 1, BudgetAmount = 500000, ActualAmount = 480000, Status = "Approved",  UpdatedAt = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Budget { Id = 2, Brand = "LuxBag", Channel = "電商", Year = 2026, Month = 1, BudgetAmount = 300000, ActualAmount = 310000, Status = "Approved",  UpdatedAt = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Budget { Id = 3, Brand = "SportX", Channel = "百貨", Year = 2026, Month = 1, BudgetAmount = 400000, ActualAmount = 390000, Status = "Approved",  UpdatedAt = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Budget { Id = 4, Brand = "LuxBag", Channel = "百貨", Year = 2026, Month = 2, BudgetAmount = 550000, ActualAmount = 520000, Status = "Reviewing", UpdatedAt = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Budget { Id = 5, Brand = "SportX", Channel = "電商", Year = 2026, Month = 2, BudgetAmount = 350000, ActualAmount = 0,      Status = "Draft",     UpdatedAt = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
