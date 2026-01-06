using OrderingSystem.BusinessLogic.Interfaces;
using OrderingSystem.DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace OrderingSystem.DataValidation.Services;

public class PaymentValidationService : IPaymentValidationService
{
    private readonly IPaymentService _paymentService;

    public PaymentValidationService(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<Payment?> GetPaymentAsync(Guid id)
    {
        return await _paymentService.GetPaymentAsync(id);
    }
}
