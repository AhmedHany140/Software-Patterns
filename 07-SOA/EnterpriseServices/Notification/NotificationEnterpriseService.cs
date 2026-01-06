using System;
using System.Threading.Tasks;

namespace OrderingSystem.EnterpriseServices.Notification;

public class NotificationEnterpriseService
{
    // Simulating a shared enterprise service for emails
    public Task SendEmailAsync(string to, string subject, string body, string? bcc = null, byte[]? attachment = null)
    {
        // Simulate sending email
        Console.WriteLine($"[Enterprise Notification Service] Sending Email to: {to}");
        Console.WriteLine($"[Subject]: {subject}");
        
        if (!string.IsNullOrEmpty(bcc))
        {
            Console.WriteLine($"[BCC]: {bcc}");
        }

        if (attachment != null && attachment.Length > 0)
        {
             Console.WriteLine($"[Attachment]: Included ({attachment.Length} bytes)");
        }

        return Task.CompletedTask;
    }
}
