using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.Pipeline.Core;

public interface IPipelineStep<TContext>
{
    Task ExecuteAsync(TContext context);
}
