using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataAccess.Repositories;
using SharedEvents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.Handlers;

public class InventoryHandler : INotificationHandler<PaymentProcessedEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public InventoryHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task Handle(PaymentProcessedEvent notification, CancellationToken cancellationToken)
    {
        if (!notification.IsSuccessful) return;

        using var scope = _scopeFactory.CreateScope();
        var productRepo = scope.ServiceProvider.GetRequiredService<IRepository<Product>>();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        var product = await productRepo.GetByIdAsync(notification.ProductId);
        if (product != null)
        {
            if (product.StockQuantity >= notification.Quantity)
            {
                product.StockQuantity -= notification.Quantity;
                productRepo.Update(product);
                await productRepo.SaveChangesAsync();
                
                // Technically could publish StockReservedEvent here
            }
            else
            {
                // Stock depletion issue despite initial check
                await publisher.Publish(new StockDepletedEvent(product.Id, notification.OrderId), cancellationToken);
                
                // In real world, we might need to Refund or Cancel Order here.
                // But keeping scope simple as per instructions.
            }
        }
    }
}
