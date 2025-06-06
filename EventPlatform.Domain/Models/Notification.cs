namespace EventPlatform.Domain.Models
{
    public enum NotificationType
    {
        Info,
        Warn,
        Error
    }

    public class Notification
    {
        public Guid Id { get; set; }
        public string Subject { get; set; } // например: "Возврат билета", "Мероприятие отклонено"
        public string Text { get; set; }
        public NotificationType Type { get; set; }

    }
}
