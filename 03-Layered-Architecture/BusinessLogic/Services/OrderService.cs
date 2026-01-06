using OrderingSystem.BusinessLogic.Interfaces;
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
    private readonly IPaymentService _paymentService;

    public OrderService(
        IRepository<Order> orderRepository,
        IRepository<Product> productRepository,
        IPaymentService paymentService)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _paymentService = paymentService;
    }

    public async Task<Order> CreateOrderAsync(Guid productId, int quantity, string customerName)
    {
        // 1. Check if product exists
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            throw new ArgumentException("Product not found");
        }

        // 2. Check stock
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

        // 4. Call PaymentService
        var payment = await _paymentService.ProcessPayment(order.Id, order.TotalAmount);

        // 5. Update Order based on payment
        if (payment.Status == PaymentStatus.Completed)
        {
            order.Status = OrderStatus.Completed;
            
            // Decrease stock
            product.StockQuantity -= quantity;
            _productRepository.Update(product);
        }
        else
        {
            order.Status = OrderStatus.Cancelled;
        }

        // 6. Save Changes (Atomic Transaction)
        // Since all repositories share the same Context (Scoped), calling SaveChanges on any repo works.
        await _orderRepository.SaveChangesAsync();

        return order;
    }

    public async Task<Order?> GetOrderAsync(Guid id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }
}
