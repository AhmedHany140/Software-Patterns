using OrderingSystem.BusinessLogic.Interfaces;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.Services;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;

    public ProductService(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();
        return product;
    }
}
