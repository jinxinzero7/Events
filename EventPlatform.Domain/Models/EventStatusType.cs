namespace EventPlatform.Domain.Models
{
    public class EventStatusType
    {
        public Guid Id { get; set; }
        public string Title { get; set; } // "На модерации", "Одобрено", "Отклонено"

    }
}
