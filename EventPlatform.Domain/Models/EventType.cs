namespace EventPlatform.Domain.Models
{
    public class EventType
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool Available { get; set; } // доступен для создания новых мероприятий

    }
}
