using OrderingSystem.BusinessLogic.MicroKernel.Interfaces;
using OrderingSystem.DataAccess;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataAccess.Enums;
using OrderingSystem.DataAccess.Repositories;

namespace OrderingSystem.BusinessLogic.MicroKernel.Core;

public class OrderProcessingCore : IOrderProcessingCore
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly ApplicationDbContext _dbContext;
    private readonly IEnumerable<IOrderPlugin> _plugins;

    public OrderProcessingCore(
        IRepository<Order> orderRepository,
        IRepository<Product> productRepository,
        ApplicationDbContext dbContext,
        IEnumerable<IOrderPlugin> plugins)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _dbContext = dbContext;
        _plugins = plugins;
    }

    public async Task<Order> CreateOrderAsync(Guid productId, int quantity, string customerName)
    {
        // 1. Transaction Scope
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // 2. Core: Load Product & Validate Stock
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) 
                throw new InvalidOperationException($"Product with ID {productId} not found.");
            
            if (product.StockQuantity < quantity)
                 throw new InvalidOperationException($"Insufficient stock. Available: {product.StockQuantity}");

            // 3. Core: Create Order Object (Lifecycle: Create)
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = customerName,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalAmount = product.Price * quantity 
            };
            
            // 4. Core: Execute Plugins (The Extension Point)
            foreach (var plugin in _plugins)
            {
                await plugin.ExecuteAsync(order);
            }
            
            // 5. Core: Persist State (Lifecycle: Save)
            await _orderRepository.AddAsync(order);
            // Updating product stock as part of core logic since we validated it?
            // Usually deducting stock is core business valid outcome.
            product.StockQuantity -= quantity;
            _productRepository.Update(product);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return order;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
