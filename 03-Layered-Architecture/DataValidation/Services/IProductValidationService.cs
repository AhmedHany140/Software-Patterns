using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataValidation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderingSystem.DataValidation.Services;

public interface IProductValidationService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product> ValidateAndCreateProductAsync(CreateProductRequest request);
}
