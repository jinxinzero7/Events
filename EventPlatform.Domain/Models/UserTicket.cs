namespace EventPlatform.Domain.Models
{
    public enum TicketStatus
    {
        Активный,
        Использованный,
        Возвращен
    }
    public class UserTicket
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TicketId { get; set; }
        public TicketStatus Status { get; set; }

        // Для генерации QR-кода можно использовать Id, UserId, TicketId, EventId (через Ticket)
    }
}
