using OrderingSystem.Data;
using OrderingSystem.Modules.Payments.Entities;
using OrderingSystem.Modules.Payments.Enums;
using System;
using System.Threading.Tasks;

namespace OrderingSystem.Modules.Payments.Services;

public class PaymentService : IPaymentService
{
    private readonly ApplicationDbContext _context;

    public PaymentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Payment> ProcessPayment(Guid orderId, decimal amount)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            Amount = amount,
            PaymentDate = DateTime.UtcNow
        };

        if (amount > 1000)
        {
            payment.Status = PaymentStatus.Failed;
        }
        else
        {
            payment.Status = PaymentStatus.Completed;
        }

        _context.Payments.Add(payment);
        
        // We do not SaveChanges here. We rely on the caller to SaveChanges 
        // to maintain a single transaction for the entire order process.
        
        return Task.FromResult(payment);
    }
}
