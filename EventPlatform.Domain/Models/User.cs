namespace EventPlatform.Domain.Models
{
    public enum AccountType
    {
        Organizer,
        Visitor
    }
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } // login
        public string Username { get; set; }
        public string? PasswordHash { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
