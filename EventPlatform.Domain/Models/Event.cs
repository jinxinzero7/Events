namespace EventPlatform.Domain.Models
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string PhotoId { get; set; } // ключ для картинки в кэше
        public string Description { get; set; }
        public DateTime EventTime { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string AdditionalRequirements { get; set; }
        public Guid EventTypeId { get; set; }
        public Guid StatusId { get; set; }
        public int TotalTickets { get; set; }
        public int ReturnedTickets { get; set; }

    }
}
