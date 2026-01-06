using MediatR;
using System;

namespace SharedEvents;

public record OrderCreatedEvent(Guid OrderId, Guid ProductId, int Quantity, decimal TotalAmount, string CustomerName) : INotification;

public record PaymentProcessedEvent(Guid PaymentId, Guid OrderId, Guid ProductId, int Quantity, bool IsSuccessful) : INotification;

public record StockDepletedEvent(Guid ProductId, Guid OrderId) : INotification;
