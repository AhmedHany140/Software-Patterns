using System;
using System.Threading.Tasks;

namespace OrderingSystem.EnterpriseServices.Payment;

public class PaymentEnterpriseService
{
    // Simulating an external service call
    public Task<PaymentResult> ProcessPaymentAsync(Guid orderId, decimal amount)
    {
        // Simulate processing time
        // In a real world, this would be an HTTP call to https://payment.enterprise.com/api/v1/charge
        
        // Simulating logic
        if (amount > 5000)
        {
            return Task.FromResult(new PaymentResult 
            { 
                TransactionId = Guid.NewGuid(), 
                Success = false, 
                Message = "Amount exceeds limit for auto-approval." 
            });
        }

        return Task.FromResult(new PaymentResult 
        { 
            TransactionId = Guid.NewGuid(), 
            Success = true, 
            Message = "Payment Processed Successfully." 
        });
    }
}

public class PaymentResult
{
    public Guid TransactionId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
}
