using OrderingSystem.BusinessLogic.Interfaces;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataValidation.Models;
using System;
using System.Threading.Tasks;

namespace OrderingSystem.DataValidation.Services;

public class OrderValidationService : IOrderValidationService
{
    private readonly IOrderService _orderService;

    public OrderValidationService(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<Order> ValidateAndCreateOrderAsync(CreateOrderRequest request)
    {
        if (request.Quantity <= 0)
        {
            throw new ValidationException("Quantity must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(request.CustomerName))
        {
            throw new ValidationException("Customer Name is required.");
        }

        return await _orderService.CreateOrderAsync(request.ProductId, request.Quantity, request.CustomerName);
    }

    public async Task<Order?> GetOrderAsync(Guid id)
    {
        // No validation needed for simple ID lookup, just pass through.
        return await _orderService.GetOrderAsync(id);
    }
}
