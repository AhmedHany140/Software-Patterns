using System;

namespace OrderingSystem.DataValidation.Models;

public class CreateOrderRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string CustomerName { get; set; } = string.Empty;
}
