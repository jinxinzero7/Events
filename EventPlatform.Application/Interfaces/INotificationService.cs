using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlatform.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendEmailNotification(string email, string subject, string body);
    }
}
