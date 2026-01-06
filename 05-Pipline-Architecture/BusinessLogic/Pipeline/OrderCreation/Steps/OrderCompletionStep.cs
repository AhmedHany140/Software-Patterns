using System.Threading.Tasks;
using OrderingSystem.BusinessLogic.Pipeline.Core;
using OrderingSystem.DataAccess.Repositories;
using OrderingSystem.DataAccess.Entities;
using OrderingSystem.DataAccess.Enums;

namespace OrderingSystem.BusinessLogic.Pipeline.OrderCreation.Steps;

public class OrderCompletionStep : IPipelineStep<OrderCreationContext>
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Order> _orderRepository;

    public OrderCompletionStep(IRepository<Product> productRepository, IRepository<Order> orderRepository)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    public async Task ExecuteAsync(OrderCreationContext context)
    {
        if (!context.IsSuccessful) return;

        // Decrease Stock
        context.Product!.StockQuantity -= context.Quantity;
        _productRepository.Update(context.Product);

        // Set Order to Completed
        context.Order!.Status = OrderStatus.Completed;
        _orderRepository.Update(context.Order);
        
        // SaveChanges will be called by Orchestrator
    }
}
