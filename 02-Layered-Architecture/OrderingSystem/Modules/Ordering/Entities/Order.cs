using OrderingSystem.Modules.Ordering.Enums;
using System;

namespace OrderingSystem.Modules.Ordering.Entities;

public class Order
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime OrderDate { get; set; }
}
