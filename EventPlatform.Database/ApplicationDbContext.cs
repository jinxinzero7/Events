using Microsoft.EntityFrameworkCore;
using EventPlatform.Domain.Models;

namespace EventPlatform.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка отношений между сущностями
            base.OnModelCreating(modelBuilder);

            // Настройка User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id); // Первичный ключ
                entity.Property(e => e.Email).IsRequired(); // Email обязателен
                entity.HasIndex(e => e.Email).IsUnique(); // Email должен быть уникальным
                entity.Property(e => e.PasswordHash).IsRequired(); // PasswordHash обязателен
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()"); // Значение по умолчанию для CreatedAt
            });

            // Настройка Event
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id); // Первичный ключ
                entity.Property(e => e.Title).IsRequired(); // Title обязателен
                entity.Property(e => e.Description).IsRequired(); // Description обязателен
                entity.Property(e => e.EventTime).IsRequired(); // EventTime обязателен
                entity.Property(e => e.Address).IsRequired(); // Address обязателен
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()"); // Значение по умолчанию для CreatedAt

                // Связь с User (один ко многим: один организатор может создать много мероприятий)
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.OrganizerId)
                    .OnDelete(DeleteBehavior.Cascade); // Cascade delete - при удалении организатора удаляются и его мероприятия
            });

            // Конфигурация для Ticket
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Связь с мероприятием
                entity.HasOne(t => t.Event)
                    .WithMany(e => e.Tickets)
                    .HasForeignKey(t => t.EventId);

                // Связь с пользователем
                entity.HasOne(t => t.User)
                    .WithMany(u => u.Tickets)
                    .HasForeignKey(t => t.UserId);
            });

            // Конфигурация для EventType
            modelBuilder.Entity<EventType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
            });

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<EventType> EventTypes { get; set; }

    }
}
