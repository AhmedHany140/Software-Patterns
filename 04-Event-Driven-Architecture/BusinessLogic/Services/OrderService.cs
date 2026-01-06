using OrderingSystem.BusinessLogic.Interfaces;
using MediatR;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataAccess.Enums;
using OrderingSystem.DataAccess.Repositories;
using System;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.Services;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IPublisher _publisher;

    public OrderService(
        IRepository<Order> orderRepository,
        IRepository<Product> productRepository,
        IPublisher publisher)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _publisher = publisher;
    }

    public async Task<Order> CreateOrderAsync(Guid productId, int quantity, string customerName)
    {
        // 1. Check if product exists
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            throw new ArgumentException("Product not found");
        }

        // 2. Check stock (Optimistic check)
        if (product.StockQuantity < quantity)
        {
            throw new InvalidOperationException("Insufficient stock");
        }

        // 3. Create Order
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = customerName,
            TotalAmount = product.Price * quantity,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending
        };

        await _orderRepository.AddAsync(order);
        
        // 4. Save initial Pending Order
        await _orderRepository.SaveChangesAsync();

        // 5. Publish Event
        await _publisher.Publish(new SharedEvents.OrderCreatedEvent(
            order.Id, 
            productId, 
            quantity, 
            order.TotalAmount, 
            customerName));

        return order;
    }

    public async Task<Order?> GetOrderAsync(Guid id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }
}
