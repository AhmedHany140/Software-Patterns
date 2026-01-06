using System.Threading.Tasks;
using OrderingSystem.BusinessLogic.Integration.Interfaces;
using OrderingSystem.EnterpriseServices.Notification;

namespace OrderingSystem.BusinessLogic.Integration.Clients;

public class EnterpriseNotificationClient : IEnterpriseNotificationClient
{
    private readonly NotificationEnterpriseService _notificationService;

    public EnterpriseNotificationClient()
    {
        _notificationService = new NotificationEnterpriseService();
    }

    public async Task NotifyAsync(string recipient, string subject, string message)
    {
        // The client creates the abstraction. The Core doesn't know about BCC or Attachments yet, 
        // but the underlying service supports it, allowing for future expansion without breaking Core.
        await _notificationService.SendEmailAsync(recipient, subject, message);
    }
}
