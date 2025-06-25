using Microsoft.EntityFrameworkCore;
using EventPlatform.Domain.Models;

namespace EventPlatform.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
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
                entity.Property(e => e.BirthDate).IsRequired(false);
                entity.Property(e => e.Phone).HasMaxLength(20).IsRequired(false); 
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
                entity.Property(e => e.EventType).IsRequired(); // EventType is now an enum

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

            // Конфигурация для EventTagType
            modelBuilder.Entity<EventTagType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
            });

            // Конфигурация для EventTag
            modelBuilder.Entity<EventTag>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(et => et.Event)
                    .WithMany()
                    .HasForeignKey(et => et.EventId)
                    .OnDelete(DeleteBehavior.Cascade); // При удалении мероприятия удаляются и связанные теги

                entity.HasOne(et => et.TagType)
                    .WithMany()
                    .HasForeignKey(et => et.TagId)
                    .OnDelete(DeleteBehavior.Cascade); // При удалении типа тега удаляются и связанные теги
            });

            // Конфигурация для EventMoodType
            modelBuilder.Entity<EventMoodType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
            });

            // Конфигурация для EventMood
            modelBuilder.Entity<EventMood>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(em => em.Event)
                    .WithMany()
                    .HasForeignKey(em => em.EventId)
                    .OnDelete(DeleteBehavior.Cascade); // При удалении мероприятия удаляются и связанные настроения

                entity.HasOne(em => em.MoodType)
                    .WithMany()
                    .HasForeignKey(em => em.MoodId)
                    .OnDelete(DeleteBehavior.Cascade); // При удалении типа настроения удаляются и связанные настроения
            });

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<EventTagType> EventTagTypes { get; set; }
        public DbSet<EventTag> EventTags { get; set; }
        public DbSet<EventMoodType> EventMoodTypes { get; set; }
        public DbSet<EventMood> EventMoods { get; set; }

    }
}
