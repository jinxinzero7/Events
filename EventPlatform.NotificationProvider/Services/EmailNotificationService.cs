using EventPlatform.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Notification.Services
{
    public class EmailNotificationService : INotificationService
    {
        public Task SendEmailNotification(string email, string subject, string body)
        {
            // Заглушка для MVP
            Console.WriteLine($"Sent email to {email}: {subject}\n{body}");
            return Task.CompletedTask;
        }
    }
}
