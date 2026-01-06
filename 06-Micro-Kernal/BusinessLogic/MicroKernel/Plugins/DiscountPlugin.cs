using OrderingSystem.BusinessLogic.MicroKernel.Interfaces;
using OrderingSystem.DataAccess.Entities;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.MicroKernel.Plugins;

public class DiscountPlugin : IOrderPlugin
{
    public Task ExecuteAsync(Order order)
    {
        if (order.TotalAmount > 500)
        {
            // Apply 10% discount
            order.TotalAmount *= 0.9m;
        }
        return Task.CompletedTask;
    }
}
