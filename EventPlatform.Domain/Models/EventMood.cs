namespace EventPlatform.Domain.Models
{
    public class EventMood
    {
        public Guid Id { get; set; }
        public Guid MoodId { get; set; }
        public Guid EventId { get; set; }

    }
}
