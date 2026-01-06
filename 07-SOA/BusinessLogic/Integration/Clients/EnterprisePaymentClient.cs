using System;
using System.Threading.Tasks;
using OrderingSystem.BusinessLogic.Integration.Interfaces;
using OrderingSystem.EnterpriseServices.Payment;

namespace OrderingSystem.BusinessLogic.Integration.Clients;

public class EnterprisePaymentClient : IEnterprisePaymentClient
{
    // In a real SOA, this would likely be an HttpClient or gRPC client.
    // Here we instantiate the "Service" directly to simulate the call, but keep it decoupled via this client wrapper.
    private readonly PaymentEnterpriseService _paymentService;

    public EnterprisePaymentClient()
    {
        _paymentService = new PaymentEnterpriseService();
    }

    public async Task<(bool Success, Guid TransactionId, string Message)> PayAsync(Guid orderId, decimal amount)
    {
        var result = await _paymentService.ProcessPaymentAsync(orderId, amount);
        return (result.Success, result.TransactionId, result.Message);
    }
}
