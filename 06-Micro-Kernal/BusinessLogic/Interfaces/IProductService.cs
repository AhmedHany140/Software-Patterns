using OrderingSystem.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<Product> CreateProductAsync(Product product);
}
