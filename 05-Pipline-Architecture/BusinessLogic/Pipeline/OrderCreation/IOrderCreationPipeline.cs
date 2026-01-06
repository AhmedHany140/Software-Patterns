using System.Threading.Tasks;
using OrderingSystem.DataAccess.Entities;

namespace OrderingSystem.BusinessLogic.Pipeline.OrderCreation;

public interface IOrderCreationPipeline
{
    Task<Order> ExecuteAsync(Guid productId, int quantity, string customerName);
}
