using OrderingSystem.DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.MicroKernel.Interfaces;

public interface IOrderProcessingCore
{
    Task<Order> CreateOrderAsync(Guid productId, int quantity, string customerName);
}
