using System;
using System.Threading.Tasks;
using OrderingSystem.DataAccess.Entities;

namespace OrderingSystem.BusinessLogic.Interfaces;

public interface IPaymentService
{
    Task<Payment> ProcessPayment(Guid orderId, decimal amount);
    Task<Payment?> GetPaymentAsync(Guid id);
}
