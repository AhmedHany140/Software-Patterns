using OrderingSystem.DataAccess.Entities;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.MicroKernel.Interfaces;

public interface IOrderPlugin
{
    Task ExecuteAsync(Order order);
}
