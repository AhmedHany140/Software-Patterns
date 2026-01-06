using System;
using System.Threading.Tasks;
using OrderingSystem.BusinessLogic.Pipeline.Core;
using OrderingSystem.DataAccess.Repositories;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataAccess.Enums;

namespace OrderingSystem.BusinessLogic.Pipeline.OrderCreation.Steps;

public class OrderInitializationStep : IPipelineStep<OrderCreationContext>
{
    private readonly IRepository<Order> _orderRepository;

    public OrderInitializationStep(IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task ExecuteAsync(OrderCreationContext context)
    {
        if (!context.IsSuccessful) return;

        var order = new Order
        {
            Id = context.OrderId,
            CustomerName = context.CustomerName,
            TotalAmount = context.Product!.Price * context.Quantity,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending
        };

        await _orderRepository.AddAsync(order);
        context.Order = order;
        
        // Note: We are not calling SaveChangesAsync here because the pipeline orchestrator handles the transaction commit.
    }
}
