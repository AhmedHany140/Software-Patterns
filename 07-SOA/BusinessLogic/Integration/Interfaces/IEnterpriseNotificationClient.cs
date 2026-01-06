using System.Threading.Tasks;

namespace OrderingSystem.BusinessLogic.Integration.Interfaces;

public interface IEnterpriseNotificationClient
{
    Task NotifyAsync(string recipient, string subject, string message);
}
