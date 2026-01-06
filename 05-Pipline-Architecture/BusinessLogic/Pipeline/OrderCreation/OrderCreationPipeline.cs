using System;
using System.Linq;
using System.Threading.Tasks;
using OrderingSystem.BusinessLogic.Pipeline.OrderCreation.Steps;
using OrderingSystem.DataAccess;
using OrderingSystem.DataAccess.Entities;

namespace OrderingSystem.BusinessLogic.Pipeline.OrderCreation;

public class OrderCreationPipeline : IOrderCreationPipeline
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ValidationStep _validationStep;
    private readonly OrderInitializationStep _initializationStep;
    private readonly PaymentStep _paymentStep;
    private readonly OrderCompletionStep _completionStep;

    public OrderCreationPipeline(
        ApplicationDbContext dbContext,
        ValidationStep validationStep,
        OrderInitializationStep initializationStep,
        PaymentStep paymentStep,
        OrderCompletionStep completionStep)
    {
        _dbContext = dbContext;
        _validationStep = validationStep;
        _initializationStep = initializationStep;
        _paymentStep = paymentStep;
        _completionStep = completionStep;
    }

    public async Task<Order> ExecuteAsync(Guid productId, int quantity, string customerName)
    {
        var context = new OrderCreationContext(productId, quantity, customerName);

        // Run entire pipeline within a single transaction
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // Step 1: Validation
            await _validationStep.ExecuteAsync(context);
            if (!context.IsSuccessful) goto Finish;

            // Step 2: Initialization
            await _initializationStep.ExecuteAsync(context);
            if (!context.IsSuccessful) goto Finish;

            // Step 3: Payment
            await _paymentStep.ExecuteAsync(context);
            if (!context.IsSuccessful) goto Finish;

            // Step 4: Completion
            await _completionStep.ExecuteAsync(context);
            if (!context.IsSuccessful) goto Finish;

            // If we are here, everything is successful. 
            // Save changes to DB. The repositories used `AddAsync` / `Update` on the Context entities.
            // Since all Repositories share the same DbContext (Scoped), saving here commits all changes.
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return context.Order!;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }

        Finish:
        // Handle failure
        await transaction.RollbackAsync();
        var errorMessage = string.Join("; ", context.Errors);
        throw new InvalidOperationException($"Order creation failed: {errorMessage}");
    }
}
