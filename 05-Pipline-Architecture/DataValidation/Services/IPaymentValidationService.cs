using OrderingSystem.DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace OrderingSystem.DataValidation.Services;

public interface IPaymentValidationService
{
    Task<Payment?> GetPaymentAsync(Guid id);
}
