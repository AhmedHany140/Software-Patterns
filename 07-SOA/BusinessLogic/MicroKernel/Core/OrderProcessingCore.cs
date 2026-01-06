using OrderingSystem.BusinessLogic.Integration.Interfaces;
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
    
    // Enterprise Service Clients (SOA)
    private readonly IEnterprisePaymentClient _paymentClient;
    private readonly IEnterpriseNotificationClient _notificationClient;

    // We can still keep general purpose plugins if needed (e.g., Discount/Tax), 
    // but Payment/Notification are now rigid Enterprise Services.
    private readonly IEnumerable<IOrderPlugin> _plugins;

    public OrderProcessingCore(
        IRepository<Order> orderRepository,
        IRepository<Product> productRepository,
        ApplicationDbContext dbContext,
        IEnterprisePaymentClient paymentClient,
        IEnterpriseNotificationClient notificationClient,
        IEnumerable<IOrderPlugin> plugins)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _dbContext = dbContext;
        _paymentClient = paymentClient;
        _notificationClient = notificationClient;
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
            
            // 4. Core: Execute Business Logic Plugins (Discount/Tax)
            // Payment logic is REMOVED from here as a plugin using IOrderPlugin for generic purposes,
            // but we will keep the loop for other business rules (Tax/Discount).
            foreach (var plugin in _plugins)
            {
                // We assume PaymentPlugin is removed from DI, so it won't run here.
                await plugin.ExecuteAsync(order);
            }
            
            // 5. SOA Integration: Call Enterprise Payment Service
            // This is now an explicit step in the workflow, not a generic plugin.
            var paymentResult = await _paymentClient.PayAsync(order.Id, order.TotalAmount);
            if (!paymentResult.Success)
            {
                 throw new InvalidOperationException($"Enterprise Payment Failed: {paymentResult.Message}");
            }
            // Note: In real SOA, we might save a Payment Reference ID here.
            
            // 6. SOA Integration: Call Enterprise Notification Service
            await _notificationClient.NotifyAsync("customer@example.com", "Order Confirmation", $"Your order {order.Id} is confirmed.");

            // 7. Core: Persist State (Lifecycle: Save)
            await _orderRepository.AddAsync(order);
            
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
