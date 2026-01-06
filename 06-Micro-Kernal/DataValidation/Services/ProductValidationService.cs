using OrderingSystem.BusinessLogic.Interfaces;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataValidation.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderingSystem.DataValidation.Services;

public class ProductValidationService : IProductValidationService
{
    private readonly IProductService _productService;

    public ProductValidationService(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productService.GetProductsAsync();
    }

    public async Task<Product> ValidateAndCreateProductAsync(CreateProductRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            throw new ValidationException("Product Name is required.");
        }
        if (request.Price <= 0)
        {
            throw new ValidationException("Price must be greater than zero.");
        }
        if (request.StockQuantity < 0)
        {
            throw new ValidationException("Stock Quantity cannot be negative.");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            StockQuantity = request.StockQuantity
        };

        return await _productService.CreateProductAsync(product);
    }
}
