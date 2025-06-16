namespace EventPlatform.Domain.Models
{
    public enum EventStatusType
    {
        Модерируется,
        Одобрено,
        Отклонено
    }
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrganizerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventTime { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public EventStatusType Status { get; set; } = EventStatusType.Модерируется;
        public Guid EventTypeId { get; set; }
        public decimal TicketPrice { get; set; }
        public int TicketQuantity { get; set; }
        public int AvailableTickets { get; set; }

        // Навигационные свойства
        public EventType EventType { get; set; }
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
