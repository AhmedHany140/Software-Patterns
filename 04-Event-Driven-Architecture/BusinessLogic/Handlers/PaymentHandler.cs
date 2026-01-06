using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderingSystem.BusinessLogic.Interfaces;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataAccess.Enums;
using OrderingSystem.DataAccess.Repositories;
using SharedEvents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.Handlers;

public class PaymentHandler : INotificationHandler<OrderCreatedEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PaymentHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Create an independent scope to simulate independent processing/microservice
        using var scope = _scopeFactory.CreateScope();
        
        var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
        // Using IRepository<Payment> to save changes, as PaymentService doesn't expose Save logic
        var paymentRepo = scope.ServiceProvider.GetRequiredService<IRepository<Payment>>();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        // Process Payment
        var payment = await paymentService.ProcessPayment(notification.OrderId, notification.TotalAmount);

        // Save Payment
        await paymentRepo.SaveChangesAsync();

        // Publish Event
        if (payment.Status == PaymentStatus.Completed)
        {
            await publisher.Publish(new PaymentProcessedEvent(
                payment.Id,
                notification.OrderId,
                notification.ProductId,
                notification.Quantity,
                true
            ), cancellationToken);
        }
        else
        {
             // Optionally publish Failed event, or Processed with Success=False
             await publisher.Publish(new PaymentProcessedEvent(
                payment.Id,
                notification.OrderId,
                notification.ProductId,
                notification.Quantity,
                false
            ), cancellationToken);
        }
    }
}
