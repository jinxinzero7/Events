namespace EventPlatform.Domain.Models
{
    public class UserEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
    }
}
