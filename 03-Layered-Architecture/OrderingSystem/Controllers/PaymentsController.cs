using Microsoft.AspNetCore.Mvc;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataValidation.Services;
using System;
using System.Threading.Tasks;

namespace UserInterface.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentValidationService _paymentValidationService;

    public PaymentsController(IPaymentValidationService paymentValidationService)
    {
        _paymentValidationService = paymentValidationService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Payment>> GetPayment(Guid id)
    {
        var payment = await _paymentValidationService.GetPaymentAsync(id);

        if (payment == null)
        {
            return NotFound();
        }

        return payment;
    }
}
