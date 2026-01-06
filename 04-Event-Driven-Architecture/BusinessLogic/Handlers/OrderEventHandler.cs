using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataAccess.Enums;
using OrderingSystem.DataAccess.Repositories;
using SharedEvents;
using System.Threading;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.Handlers;

public class OrderEventHandler : 
    INotificationHandler<PaymentProcessedEvent>,
    INotificationHandler<StockDepletedEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public OrderEventHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task Handle(PaymentProcessedEvent notification, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var orderRepo = scope.ServiceProvider.GetRequiredService<IRepository<Order>>();
        
        var order = await orderRepo.GetByIdAsync(notification.OrderId);
        if (order != null)
        {
            if (notification.IsSuccessful)
            {
                // We assume Completed for now. If stock fails later, another event will compensate.
                order.Status = OrderStatus.Completed; 
            }
            else
            {
                order.Status = OrderStatus.Cancelled;
            }
            
            orderRepo.Update(order);
            await orderRepo.SaveChangesAsync();
        }
    }

    public async Task Handle(StockDepletedEvent notification, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var orderRepo = scope.ServiceProvider.GetRequiredService<IRepository<Order>>();
        
        var order = await orderRepo.GetByIdAsync(notification.OrderId);
        if (order != null)
        {
            // Compensation logic: Stock ran out after initial check?
            order.Status = OrderStatus.Cancelled;
            orderRepo.Update(order);
            await orderRepo.SaveChangesAsync();
            
            // TODO: Issue Refund since Payment was processed.
        }
    }
}
