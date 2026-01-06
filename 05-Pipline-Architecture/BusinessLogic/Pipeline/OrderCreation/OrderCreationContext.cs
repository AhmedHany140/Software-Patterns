using System;
using System.Collections.Generic;
using OrderingSystem.DataAccess.Entities;

namespace OrderingSystem.BusinessLogic.Pipeline.OrderCreation;

public class OrderCreationContext
{
    // Input
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string CustomerName { get; set; }

    // State
    public Product? Product { get; set; }
    public Order? Order { get; set; }
    public Guid OrderId { get; set; } // Generated early or in Init step
    public bool IsSuccessful { get; set; } = true;
    public List<string> Errors { get; set; } = new();

    public OrderCreationContext(Guid productId, int quantity, string customerName)
    {
        ProductId = productId;
        Quantity = quantity;
        CustomerName = customerName;
        OrderId = Guid.NewGuid(); // Generate ID early if needed
    }

    public void Fail(string error)
    {
        IsSuccessful = false;
        Errors.Add(error);
    }
}
