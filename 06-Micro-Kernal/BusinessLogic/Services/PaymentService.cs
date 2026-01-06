using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataAccess.Enums;
using OrderingSystem.DataAccess.Repositories;
using OrderingSystem.BusinessLogic.Interfaces;
using System;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.Services;

public class PaymentService : IPaymentService
{
    private readonly IRepository<Payment> _paymentRepository;

    public PaymentService(IRepository<Payment> paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<Payment> ProcessPayment(Guid orderId, decimal amount)
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
            payment.Status = PaymentStatus.Completed; // Updated to match Enum name in previous snapshot
        }

        await _paymentRepository.AddAsync(payment);
        
        // We do not SaveChanges here. We rely on the caller to SaveChanges 
        // to maintain a single transaction for the entire order process.
        // Wait: If I call GetPaymentAsync from Controller, I might need SaveChanges if I created one previously?
        // But ProcessPayment is part of Order Workflow.
        // For GetPayment, it is read-only.
        
        return payment;
    }
    
    public async Task<Payment?> GetPaymentAsync(Guid id)
    {
        return await _paymentRepository.GetByIdAsync(id);
    }
}
