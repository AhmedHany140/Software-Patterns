using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Data;
using OrderingSystem.Modules.Ordering.Entities;
using OrderingSystem.Modules.Catalog.Entities;

namespace OrderingSystem.Modules.Ordering.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly OrderingSystem.Modules.Payments.Services.IPaymentService _paymentService;

    public OrdersController(ApplicationDbContext context, OrderingSystem.Modules.Payments.Services.IPaymentService paymentService)
    {
        _context = context;
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(int productId, int quantity, string customerName)
    {
        // 1. Check if product exists
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            return NotFound("Product not found");
        }

        // 2. Check stock
        if (product.StockQuantity < quantity)
        {
            return BadRequest("Insufficient stock");
        }

        // 3. Create Order in Pending status
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = customerName,
            TotalAmount = product.Price * quantity,
            OrderDate = DateTime.UtcNow,
            Status = "Pending"
        };

        _context.Orders.Add(order);

        // 4. Call PaymentService
        // Shared transaction scope via shared DbContext
        var payment = await _paymentService.ProcessPayment(order.Id, order.TotalAmount);

        // 5. Update Order based on payment result
        if (payment.Status == "Success")
        {
            order.Status = "Completed";
            
            // Decrease stock only if payment success
            product.StockQuantity -= quantity;
            _context.Entry(product).State = EntityState.Modified;
        }
        else
        {
            order.Status = "Cancelled";
        }

        // 6. Save Changes (Atomic Transaction: Order, Payment, Product Update)
        await _context.SaveChangesAsync();

        if (order.Status == "Cancelled")
        {
            return BadRequest($"Payment Failed. Status: {payment.Status}");
        }

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }
}
