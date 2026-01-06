using OrderingSystem.BusinessLogic.Interfaces;
using OrderingSystem.BusinessLogic.MicroKernel.Interfaces;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataAccess.Enums;
using System;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.MicroKernel.Plugins;

public class PaymentPlugin : IOrderPlugin
{
    private readonly IPaymentService _paymentService;

    public PaymentPlugin(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task ExecuteAsync(Order order)
    {
        var payment = await _paymentService.ProcessPayment(order.Id, order.TotalAmount);
        
        if (payment.Status != PaymentStatus.Completed)
        {
             throw new InvalidOperationException($"Payment failed. Status: {payment.Status}");
        }
    }
}
