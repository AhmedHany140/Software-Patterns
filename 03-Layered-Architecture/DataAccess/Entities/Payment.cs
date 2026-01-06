using OrderingSystem.DataAccess.Enums;
using System;

namespace OrderingSystem.DataAccess.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending; // Pending, Success, Failed
    public DateTime PaymentDate { get; set; }
}
