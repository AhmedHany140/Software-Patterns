using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Data;
using OrderingSystem.Models;

namespace OrderingSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
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

        // 3. Deduct stock and update product
        product.StockQuantity -= quantity;
        _context.Entry(product).State = EntityState.Modified; // Explicitly marking as modified (optional but clear)

        // 4. Create and save order
        var order = new Order
        {
            CustomerName = customerName,
            TotalAmount = product.Price * quantity,
            OrderDate = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        
        // Save changes for both Product update and new Order in one transaction (default EF behavior)
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }
}
