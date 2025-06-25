namespace EventPlatform.Domain.Models
{
    public class EventMood
    {
        public Guid Id { get; set; }
        public Guid MoodId { get; set; }
        public Guid EventId { get; set; }

        // Navigation properties
        public Event Event { get; set; }
        public EventMoodType MoodType { get; set; }

    }
}
