using DemoProject.Core.Models.Dto;
using DemoProject.Core.Models.Entities;

namespace DemoProject.Core.Services;

public interface IProductService
{
    List<Product> GetAll();
    Product? GetById(int id);
    Product Create(ProductDto dto);
    Product? Update(int id, ProductDto dto);
    bool Delete(int id);
}
