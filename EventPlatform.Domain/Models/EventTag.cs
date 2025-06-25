namespace EventPlatform.Domain.Models
{
    public class EventTag
    {
        public Guid Id { get; set; }
        public Guid TagId { get; set; }
        public Guid EventId { get; set; }

        // Navigation properties
        public Event Event { get; set; }
        public EventTagType TagType { get; set; }

    }
}
