using OrderingSystem.BusinessLogic.Interfaces;
using OrderingSystem.BusinessLogic.MicroKernel.Interfaces;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataAccess.Repositories;
using System;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.Services;

public class OrderService : IOrderService
{
    private readonly IOrderProcessingCore _orderProcessingCore;
    private readonly IRepository<Order> _orderRepository;

    public OrderService(
        IOrderProcessingCore orderProcessingCore,
        IRepository<Order> orderRepository)
    {
        _orderProcessingCore = orderProcessingCore;
        _orderRepository = orderRepository;
    }

    public async Task<Order> CreateOrderAsync(Guid productId, int quantity, string customerName)
    {
        return await _orderProcessingCore.CreateOrderAsync(productId, quantity, customerName);
    }

    public async Task<Order?> GetOrderAsync(Guid id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }
}
