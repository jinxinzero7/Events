namespace EventPlatform.Domain.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public Guid EventId { get; set; }

    }
}
