namespace EventPlatform.Domain.Models
{
    public enum AccountType
    {
        Организатор,
        Посетитель
    }
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } // login
        public string Nickname { get; set; }
        public DateTime Birthdate { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public AccountType AccountType { get; set; }
    }
}
