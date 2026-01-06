using OrderingSystem.BusinessLogic.MicroKernel.Interfaces;
using OrderingSystem.DataAccess.Entities;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.MicroKernel.Plugins;

public class TaxPlugin : IOrderPlugin
{
    public Task ExecuteAsync(Order order)
    {
        // Apply 15% Tax
        order.TotalAmount *= 1.15m;
        return Task.CompletedTask;
    }
}
