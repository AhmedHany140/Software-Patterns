using System;
using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.Integration.Interfaces;

public interface IEnterprisePaymentClient
{
    Task<(bool Success, Guid TransactionId, string Message)> PayAsync(Guid orderId, decimal amount);
}
