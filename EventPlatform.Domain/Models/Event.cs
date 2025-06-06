namespace EventPlatform.Domain.Models
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrganizerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventTime { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
