using OrderingSystem.BusinessLogic.Interfaces;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataAccess.Repositories;
using OrderingSystem.BusinessLogic.Pipeline.OrderCreation;
using System;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.Services;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IOrderCreationPipeline _orderCreationPipeline;

    public OrderService(
        IRepository<Order> orderRepository,
        IOrderCreationPipeline orderCreationPipeline)
    {
        _orderRepository = orderRepository;
        _orderCreationPipeline = orderCreationPipeline;
    }

    public async Task<Order> CreateOrderAsync(Guid productId, int quantity, string customerName)
    {
        return await _orderCreationPipeline.ExecuteAsync(productId, quantity, customerName);
    }

    public async Task<Order?> GetOrderAsync(Guid id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }
}
