using System;
using System.Threading.Tasks;
using OrderingSystem.Modules.Payments.Entities;

namespace OrderingSystem.Modules.Payments.Services;

public interface IPaymentService
{
    Task<Payment> ProcessPayment(Guid orderId, decimal amount);
}
