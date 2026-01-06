using System;
using System.Threading.Tasks;
using OrderingSystem.BusinessLogic.Pipeline.Core;
using OrderingSystem.DataAccess.Repositories;
using OrderingSystem.DataAccess.Entities;

namespace OrderingSystem.BusinessLogic.Pipeline.OrderCreation.Steps;

public class ValidationStep : IPipelineStep<OrderCreationContext>
{
    private readonly IRepository<Product> _productRepository;

    public ValidationStep(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task ExecuteAsync(OrderCreationContext context)
    {
        var product = await _productRepository.GetByIdAsync(context.ProductId);
        if (product == null)
        {
            context.Fail($"Product with ID {context.ProductId} not found.");
            return;
        }

        if (product.StockQuantity < context.Quantity)
        {
            context.Fail($"Insufficient stock for product {product.Name}. Requested: {context.Quantity}, Available: {product.StockQuantity}");
            return;
        }

        context.Product = product;
    }
}
