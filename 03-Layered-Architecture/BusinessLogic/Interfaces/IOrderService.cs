using OrderingSystem.DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(Guid productId, int quantity, string customerName);
    Task<Order?> GetOrderAsync(Guid id);
}
