using Microsoft.AspNetCore.Mvc;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataValidation;
using OrderingSystem.DataValidation.Models;
using OrderingSystem.DataValidation.Services;
using System;
using System.Threading.Tasks;

namespace UserInterface.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderValidationService _orderValidationService;

    public OrdersController(IOrderValidationService orderValidationService)
    {
        _orderValidationService = orderValidationService;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(CreateOrderRequest request)
    {
        try
        {
            var order = await _orderValidationService.ValidateAndCreateOrderAsync(request);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(Guid id)
    {
        var order = await _orderValidationService.GetOrderAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }
}
