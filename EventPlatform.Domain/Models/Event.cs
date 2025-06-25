namespace EventPlatform.Domain.Models
{
    public enum EventStatusType
    {
        Moderated,
        Approved,
        Rejected
    }
    public class Event
    {
        public Guid Id { get; set; }
        public Guid OrganizerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventTime { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public EventStatusType Status { get; set; } = EventStatusType.Moderated;
        public EventType EventType { get; set; }
        public decimal TicketPrice { get; set; }
        public int TicketQuantity { get; set; }
        public int AvailableTickets { get; set; }

        // Навигационные свойства
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
