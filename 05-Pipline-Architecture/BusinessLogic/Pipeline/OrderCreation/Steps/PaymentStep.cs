using System.Threading.Tasks;
using OrderingSystem.BusinessLogic.Pipeline.Core;
using OrderingSystem.BusinessLogic.Interfaces;
using OrderingSystem.DataAccess.Enums;

namespace OrderingSystem.BusinessLogic.Pipeline.OrderCreation.Steps;

public class PaymentStep : IPipelineStep<OrderCreationContext>
{
    private readonly IPaymentService _paymentService;

    public PaymentStep(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task ExecuteAsync(OrderCreationContext context)
    {
        if (!context.IsSuccessful) return;

        // Step 3: Invoke Payment Module
        // Passing OrderId and calculated amount from previous steps
        var payment = await _paymentService.ProcessPayment(context.OrderId, context.Order!.TotalAmount);

        if (payment.Status != PaymentStatus.Completed)
        {
            context.Fail($"Payment failed. Status: {payment.Status}");
            // Since we are in a transaction (Unit of Work), if we fail here, we just mark context as failed.
            // The pipeline stops, and we do NOT commit the transaction.
        }
    }
}
