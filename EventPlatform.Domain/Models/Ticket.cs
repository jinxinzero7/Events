using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventPlatform.Domain.Models
{
    public enum TicketStatus
    {
        Активный,
        Использованный,
        Возвращен
    }
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ключевая аннотация
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        public TicketStatus Status { get; set; } = TicketStatus.Активный;
        public string QrCodeData { get; set; } = Guid.NewGuid().ToString("N"); // Генерация уникального кода

        // Навигационные свойства (для EF Core)
        public Event Event { get; set; }
        public User User { get; set; }
    }
}
