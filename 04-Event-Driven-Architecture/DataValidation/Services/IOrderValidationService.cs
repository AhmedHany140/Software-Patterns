using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataValidation.Models;
using System;
using System.Threading.Tasks;

namespace OrderingSystem.DataValidation.Services;

public interface IOrderValidationService
{
    Task<Order> ValidateAndCreateOrderAsync(CreateOrderRequest request);
    Task<Order?> GetOrderAsync(Guid id);
}
