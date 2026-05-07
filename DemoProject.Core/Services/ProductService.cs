using DemoProject.Core.Data;
using DemoProject.Core.Models.Dto;
using DemoProject.Core.Models.Entities;

namespace DemoProject.Core.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db) => _db = db;

    public List<Product> GetAll() => _db.Products.ToList();

    public Product? GetById(int id) => _db.Products.Find(id);

    public Product Create(ProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name, Brand = dto.Brand,
            Category = dto.Category, UnitPrice = dto.UnitPrice,
            Stock = dto.Stock, CreatedAt = DateTime.UtcNow
        };
        _db.Products.Add(product);
        _db.SaveChanges();
        return product;
    }

    public Product? Update(int id, ProductDto dto)
    {
        var product = _db.Products.Find(id);
        if (product == null) return null;

        product.Name = dto.Name; product.Brand = dto.Brand;
        product.Category = dto.Category; product.UnitPrice = dto.UnitPrice;
        product.Stock = dto.Stock;
        _db.SaveChanges();
        return product;
    }

    public bool Delete(int id)
    {
        var product = _db.Products.Find(id);
        if (product == null) return false;

        _db.Products.Remove(product);
        _db.SaveChanges();
        return true;
    }
}
